// =====================================================================
// Copyright 2013-2018 ToolBuddy
// All rights reserved
// 
// http://www.toolbuddy.net
// =====================================================================

using UnityEngine;
using System.Collections;
using FluffyUnderware.Curvy.Generator;
using FluffyUnderware.Curvy.Generator.Modules;
using FluffyUnderware.Curvy.Controllers;
using FluffyUnderware.Curvy.Shapes;
using FluffyUnderware.DevTools;
using UnityEngine.UI;

namespace FluffyUnderware.Curvy.Examples
{

    public class InfiniteTrack : MonoBehaviour
    {
        public CurvySpline TrackSpline; // empty Spline object we create the path in
        public CurvyController Controller; // controller to use
        public Material RoadMaterial; // material for the extrusion
        public Text TxtStats; // the UI text holding the statistics

        [Positive]
        public float CurvationX = 10; // X-axis angle randomness
        [Positive]
        public float CurvationY = 10; // Y-axis angle randomness
        [Positive]
        public float CPStepSize = 20; // distance between CPs
        [Positive]
        public int HeadCP = 3; // CP's to build in front of extrusion
        [Positive]
        public int TailCP = 2; // CP's to keep behind extrusion

        [DevTools.Min(3)]
        public int Sections = 6; // # of Extrusions to use
        [DevTools.Min(1)]
        public int SectionCPCount = 2; // # of CP's to use for a single extrusion


        int mInitState = 0;
        bool mUpdateSpline;
        int mUpdateIn;

        CurvyGenerator[] mGenerators;
        int mCurrentGen;
        float lastSectionEndV;
        Vector3 mDir;

        TimeMeasure timeSpline = new TimeMeasure(30);
        TimeMeasure timeCG = new TimeMeasure(1);


        void Start()
        {
            updateStats();
        }

        void FixedUpdate()
        {
            if (mInitState == 0)
                StartCoroutine("setup");

            if (mInitState == 2 && mUpdateSpline)
                advanceTrack();

        }

        // setup everything
        IEnumerator setup()
        {
            mInitState = 1;

            mGenerators = new CurvyGenerator[Sections];

            // Add the start CP to the spline
            TrackSpline.InsertAfter(null, Vector3.zero, true);
            mDir = Vector3.forward;

            int num = TailCP + HeadCP + Sections * SectionCPCount;
            for (int i = 0; i < num; i++)
                addTrackCP();

            TrackSpline.Refresh();


            // build Curvy Generators
            for (int i = 0; i < Sections; i++)
            {
                mGenerators[i] = buildGenerator();
                mGenerators[i].name = "Generator " + i;
            }
            // and wait until they're initialized
            for (int i = 0; i < Sections; i++)
                while (!mGenerators[i].IsInitialized)
                    yield return 0;

            // let all generators do their extrusion
            for (int i = 0; i < Sections; i++)
                updateSectionGenerator(mGenerators[i], i * SectionCPCount + TailCP, (i + 1) * SectionCPCount + TailCP);

            mInitState = 2;
            mUpdateIn = SectionCPCount;
            // finally place the controller
            Controller.AbsolutePosition = TrackSpline.ControlPointsList[TailCP + 2].Distance;
        }

        // build a generator
        CurvyGenerator buildGenerator()
        {
            // Create the Curvy Generator
            CurvyGenerator gen = CurvyGenerator.Create();
            gen.AutoRefresh = false;
            // Create Modules
            InputSplinePath path = gen.AddModule<InputSplinePath>();
            InputSplineShape shape = gen.AddModule<InputSplineShape>();
            BuildShapeExtrusion extrude = gen.AddModule<BuildShapeExtrusion>();
            BuildVolumeMesh vol = gen.AddModule<BuildVolumeMesh>();
            CreateMesh msh = gen.AddModule<CreateMesh>();
            // Create Links between modules
            path.OutputByName["Path"].LinkTo(extrude.InputByName["Path"]);
            shape.OutputByName["Shape"].LinkTo(extrude.InputByName["Cross"]);
            extrude.OutputByName["Volume"].LinkTo(vol.InputByName["Volume"]);
            vol.OutputByName["VMesh"].LinkTo(msh.InputByName["VMesh"]);
            // Set module properties
            path.Spline = TrackSpline;
            path.UseCache = true;
            CSRectangle rectShape = shape.SetManagedShape<CSRectangle>();
            rectShape.Width = 20;
            rectShape.Height = 2;
            extrude.Optimize = false;
            extrude.CrossHardEdges = true;
            vol.Split = false;
            vol.SetMaterial(0, RoadMaterial);
            vol.MaterialSetttings[0].SwapUV = true;

            msh.Collider = CGColliderEnum.None;
            return gen;
        }

        // advance the track
        void advanceTrack()
        {
            timeSpline.Start();

            float pos = Controller.AbsolutePosition;
            //remove oldest section's CP
            for (int i = 0; i < SectionCPCount; i++)
            {
                pos -= TrackSpline.ControlPointsList[0].Length; // update controller's position, so the ship won't jump
                TrackSpline.Delete(TrackSpline.ControlPointsList[0], true);
            }
            // add new section's CP
            for (int i = 0; i < SectionCPCount; i++)
                addTrackCP();
            // refresh the spline, so orientation will be auto-calculated
            TrackSpline.Refresh();

            // set the controller to the old position
            Controller.AbsolutePosition = pos;
            mUpdateSpline = false;
            timeSpline.Stop();

            advanceSections();

            updateStats();
        }

        // update all CGs
        void advanceSections()
        {
            // set oldest CG to render path for new section
            CurvyGenerator cur = mGenerators[mCurrentGen++];
            int num = TrackSpline.ControlPointCount - HeadCP - 1;
            updateSectionGenerator(cur, num - SectionCPCount, num);

            if (mCurrentGen == Sections)
                mCurrentGen = 0;
        }

        void updateStats()
        {
#if UNITY_WEBGL
            TxtStats.text = System.String.Empty; //time measurement seems not reliable on WebGL, so I prefered deactivating it.
#else
            TxtStats.text = string.Format("Spline Update: {0:0.00} ms\nGenerator Update: {1:0.00} ms", timeSpline.AverageMS, timeCG.AverageMS);
#endif
        }

        // set a CG to render only a portion of a spline
        void updateSectionGenerator(CurvyGenerator gen, int startCP, int endCP)
        {
            // Set Track segment we want to use
            InputSplinePath path = gen.FindModules<InputSplinePath>(true)[0];

            path.EndCP = null; //To avoid trigger false positif in ValidateStartAndEndCps when setting StartCP
            path.StartCP = TrackSpline.ControlPointsList[startCP];
            path.EndCP = TrackSpline.ControlPointsList[endCP];

            // Set UV-Offset to match
            BuildVolumeMesh vol = gen.FindModules<BuildVolumeMesh>()[0];
            vol.MaterialSetttings[0].UVOffset.y = lastSectionEndV % 1;
            timeCG.Start();
            gen.Refresh();
            timeCG.Stop();
            // fetch the ending V to be used by next section
            CGVMesh vmesh = vol.OutVMesh.GetData<CGVMesh>();
            lastSectionEndV = vmesh.UV[vmesh.Count - 1].y;
        }

        // while we travel past CP's, we update the track
        public void Track_OnControlPointReached(CurvySplineMoveEventArgs e)
        {
            if (--mUpdateIn == 0)
            {
                mUpdateSpline = true;
                mUpdateIn = SectionCPCount;
            }
        }

        // add more CP's, rotating path by random angles
        void addTrackCP()
        {
            Vector3 p = TrackSpline.ControlPointsList[TrackSpline.ControlPointCount - 1].transform.localPosition;
            Vector3 position = TrackSpline.transform.localToWorldMatrix.MultiplyPoint3x4(p + mDir * CPStepSize);

            float rndX = Random.value * CurvationX * DTUtility.RandomSign();
            float rndY = Random.value * CurvationY * DTUtility.RandomSign();
            mDir = Quaternion.Euler(rndX, rndY, 0) * mDir;

            CurvySplineSegment newControlPoint = TrackSpline.InsertAfter(null, position, true);

            //Set the last control point of each section as an Orientation Anchor, to avoid that Control Points added beyond this point modify the dynamic orientation of previous Control Points
            if ((TrackSpline.ControlPointCount - 1 - TailCP) % SectionCPCount == 0)
                newControlPoint.SerializedOrientationAnchor = true;

        }
    }
}
