﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.MapCellEditor.Editor
{
    public class CellBrush
    {
        private MapCells _mapCells;
        private CellType _cellType;
        private float _radius = 1;

        private Shortcut_CellBrushRadius _shortcutBrushRadius;
        private bool _isLock;
        private Vector3 _positionOnLock;
        private Vector3 _lastApplyPoint;

        public CellType CellType { get => _cellType; set => _cellType = value; }
        public float Radius { get => _radius; set => _radius = Mathf.Clamp(value, 0, 15); }

        #region ctor
        public CellBrush(MapCells mapCells, CellType cellType)
        {
            _mapCells = mapCells;
            _cellType = cellType;

            _shortcutBrushRadius = new Shortcut_CellBrushRadius(this, KeyCode.B);

            EnableBrush();
        }
        #endregion

        #region Methods
        public void EnableBrush()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        public void DisableBrush()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        void OnSceneGUI(SceneView sceneView)
        {
            if (GetBrushApplyPoint(out Vector3 applyPoint))
            {
                TryApplyBrush(applyPoint);
                DrawBrushRadius(applyPoint);

                _shortcutBrushRadius.ProcessEvent(Event.current);

                // disable mouse selection in editor
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                _lastApplyPoint = applyPoint;
            }
            else
            {
                // don't throw error
                // because the current focused window is not scene, the error is thrown
                //Debug.LogErrorFormat("Cell Brush : Can't get apply point of brush.");
            }
        }

        #region Brush apply
        private void TryApplyBrush(Vector3 applyPoint)
        {
            if (_isLock)
                return;

            Event currentEvent = Event.current;

            bool leftMouseButtonDown = (currentEvent.type == EventType.MouseDown || currentEvent.type == EventType.MouseDrag) && currentEvent.button == 0;

            if (leftMouseButtonDown)
            {
                ApplyBrush(applyPoint);
            }
        }

        void ApplyBrush(Vector3 hitPoint)
        {
            if (_mapCells == null)
            {
                Debug.LogErrorFormat("Cell Brush : You must assign a map cells to draw.");
                return;
            }

            _mapCells.SetCellType_WorldPosition(hitPoint.x, hitPoint.z, _cellType, _radius);
        }
        #endregion

        #region Draw methods
        void DrawBrushRadius(Vector3 applyPoint)
        {
            Vector3 position = _isLock ? _positionOnLock : applyPoint;
            Handles.DrawWireDisc(position, Vector3.up, _radius);
        }
        #endregion

        #region Lock methods
        public void Lock()
        {
            _isLock = true;
            _positionOnLock = _lastApplyPoint;
        }

        public void Unlock()
        {
            _isLock = false;
        }
        #endregion

        #region Brush Apply Point method
        bool GetBrushApplyPoint(out Vector3 point)
        {
            if (Camera.current == null)
            {
                Debug.LogErrorFormat("Map Brush : Failed to get current camera. Abort.");
                point = Vector3.zero;
                return false;
            }

            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            if (SetRayOnYAxisZero(ray, out Vector3 output))
            {
                point = output;
                return true;
            }
            else
            {
                point = Vector3.zero;
                return false;
            }
        }

        bool SetRayOnYAxisZero(Ray ray, out Vector3 output)
        {
            //Math from http://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-plane-and-ray-disk-intersection

            //A plane can be defined as:
            //a point representing how far the plane is from the world origin
            Vector3 p_0 = Vector3.zero;

            //a normal (defining the orientation of the plane), should be negative if we are firing the ray from above
            Vector3 n = -Vector3.up;
            //We are intrerested in calculating a point in this plane called p
            //The vector between p and p0 and the normal is always perpendicular: (p - p_0) . n = 0

            //A ray to point p can be defined as: l_0 + l * t = p, where:
            //the origin of the ray
            Vector3 l_0 = ray.origin;
            //l is the direction of the ray
            Vector3 l = ray.direction;
            //t is the length of the ray, which we can get by combining the above equations:
            //t = ((p_0 - l_0) . n) / (l . n)

            //But there's a chance that the line doesn't intersect with the plane, and we can check this by first
            //calculating the denominator and see if it's not small. 
            //We are also checking that the denominator is positive or we are looking in the opposite direction
            float denominator = Vector3.Dot(l, n);

            if (denominator > 0.00001f)
            {
                //The distance to the plane
                float t = Vector3.Dot(p_0 - l_0, n) / denominator;

                //Where the ray intersects with a plane
                Vector3 p = l_0 + l * t;

                //Display the ray with a line renderer
                //lineRenderer.SetPosition(0, p);
                //lineRenderer.SetPosition(1, l_0);
                output = p;
                return true;
            }
            else
            {
                output = Vector3.zero;
                return false;
            }

        }
        #endregion
        #endregion
    }
}
