using OpenFace.models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenFace.persistence
{
    public class FaceMap
    {
        private static Dictionary<string, FaceCacheModel> map = new Dictionary<string, FaceCacheModel>();

        public static void Add(String id, FaceCacheModel model)
        {
            map[id] = model;
        }

        public static FaceCacheModel GetModel(String id)
        {
            return map[id];
        }

        public static void SetResult(String id, Bitmap bitmap)
        {
            map[id].Result = bitmap;
        }

        public static Bitmap GetResult(string id)
        {
            return map[id].Result;
        }
    }
}
