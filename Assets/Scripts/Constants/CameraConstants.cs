using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Constants {
    /// <summary>
    /// Constants used for camera-related tasks.
    /// </summary>
    public static class CameraConstants {
        public static readonly int STANDARD_CAMERA_DISTANCE_MULT = 125; //smaller is closer to the vehicle
        public static readonly Vector3 STANDARD_CAMERA_HEIGHT = new Vector3(0, 90, 0); //smaller is closer to the ground
        public static readonly Vector3 BIRDS_EYE_POSITION = new Vector3(0, 1454, 0); //looking down at the whole arena from a specific height
        public static readonly Vector3 SCENE_ORIGIN = new Vector3(0, 0, 0); // the origin point for the scene's xyz plane
        public static readonly Quaternion BIRDS_EYE_ROTATION = Quaternion.LookRotation(SCENE_ORIGIN - BIRDS_EYE_POSITION);  //make sure the camera points to the origin
    }
}
