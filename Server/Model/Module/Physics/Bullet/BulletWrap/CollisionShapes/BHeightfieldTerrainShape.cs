// using System;
// using UnityEngine;
// using System.Collections;
// using System.Diagnostics;
// using BulletSharp;
// using System.Runtime.InteropServices;
//
// namespace ETModel 
// {
//     [ObjectSystem]
//     public class BHeightfieldTerrainShapeComponentAwakeSystem : AwakeSystem<BHeightfieldTerrainShape>
//     {
//         public override void Awake(BHeightfieldTerrainShape self)
//         {
//             self.GetParent<Unit>().GetComponent<BCollisionShape>().CopyCollisionShape= self.thisCopyCollisionShape();
//             self.GetParent<Unit>().GetComponent<BCollisionShape>().GetCollisionShape= self.thisGetCollisionShape();
//         }
//     }
//     public class BHeightfieldTerrainShape : BCollisionShape {
//
//         public int upIndex;
//         GCHandle pinnedTerrainData;
//         PhyScalarType scalarType = PhyScalarType.Float;
//
//         public void Awake()
//         {
//             Terrain t =this.GetParent<Unit>().GetComponent<Terrain>();
//             if (t == null)
//             {
//                 Log.Warning("BHeightfieldTerrainShape must be attached to an object with a terrain." + name);
//             }
//         }
//
//         public override void OnDrawGizmosSelected()
//         {
//             
//         }
//
//         HeightfieldTerrainShape _CreateTerrainShape()
//         {
//             Terrain t =this.GetParent<Unit>().GetComponent<Terrain>();
//             if (t == null)
//             {
//                 Log.Warning("Needs to be attached to a game object with a Terrain component." + name);
//                 return null;
//             }
//             BTerrainCollisionObject tco =this.GetParent<Unit>().GetComponent<BTerrainCollisionObject>();
//             if (tco == null)
//             {
//                 Log.Warning("Needs to be attached to a game object with a BTerrainCollisionObject." + name);
//             }
//             // TerrainData td = t.terrainData;
//             // int width = td.heightmapWidth;
//             // int length = td.heightmapHeight;
//             // float maxHeight = td.size.y;
//
//             //generate procedural data
//             byte[] terr = new byte[width * length * sizeof(float)];
//             System.IO.MemoryStream file = new System.IO.MemoryStream(terr);
//             System.IO.BinaryWriter writer = new System.IO.BinaryWriter(file);
//
//             for (int i = 0; i < length; i++)
//             {
//                 float[,] row = td.GetHeights(0, i, width, 1);
//                 for (int j = 0; j < width; j++)
//                 {
//                     writer.Write((float)row[0, j] * maxHeight);
//                 }
//             }
//
//             writer.Flush();
//             file.Position = 0;
//
//             pinnedTerrainData = GCHandle.Alloc(terr, GCHandleType.Pinned);
//
//             HeightfieldTerrainShape hs = new HeightfieldTerrainShape(width, length, pinnedTerrainData.AddrOfPinnedObject(), 1f, 0f, maxHeight, upIndex, scalarType, false);
//             hs.SetUseDiamondSubdivision(true);
//             hs.LocalScaling = new BulletSharp.Math.Vector3(td.heightmapScale.x, 1f, td.heightmapScale.z);
//             //just allocated several hundred float arrays. Garbage collect now since 99% likely we just loaded the scene
//             GC.Collect();
//             return hs;
//         }
//
//         public CollisionShape thisCopyCollisionShape()
//         {
//             return _CreateTerrainShape();
//         }
//
//         public CollisionShape thisGetCollisionShape() 
//         {
//             if (collisionShapePtr == null) 
//             {
//                 collisionShapePtr = _CreateTerrainShape();
//             }
//             return collisionShapePtr;
//         }
//
//         protected override void Dispose(bool isdisposing)
//         {
//             if (collisionShapePtr != null)
//             {
//                 collisionShapePtr.Dispose();
//                 collisionShapePtr = null;
//             }
//             pinnedTerrainData.Free();
//         }
//     }
// }
