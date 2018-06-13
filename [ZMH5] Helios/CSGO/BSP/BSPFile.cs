using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Core;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.BSP
{
    //TODO: Test!
    public class BSPFile
    {
        #region VARIABLES
        public dheader_t m_BSPHeader;
        public mvertex_t[] m_Vertexes;
        public cplane_t[] m_Planes;
        public dedge_t[] m_Edges;
        public int[] m_Surfedges;
        public dleaf_t[] m_Leaves;
        public snode_t[] m_Nodes;
        public dface_t[] m_Surfaces;
        public texinfo_t[] m_Texinfos;
        public dbrush_t[] m_Brushes;
        public dbrushside_t[] m_Brushsides;
        public ushort[] m_Leaffaces;
        public ushort[] m_Leafbrushes;
        public Polygon[] m_Polygons;
        public dface_t[] m_OriginalFaces;
        public long m_PlanesAddress;
        private string m_EntitiesASCII;
        public string[] m_StaticPropsModelNames;
        public StaticPropLump_t[] m_StaticProps;
        public dgamelump_t[] m_GameLumps;
        #endregion

        public BSPFile(Stream str)
        {
            Parse(str);
        }

        #region METHODS
        private void Parse(Stream str)
        {
            m_BSPHeader = str.Read<dheader_t>();
            if (m_BSPHeader.m_Version != BSPFlags.BSPVERSION)
                Program.Logger.Warning("[BSP] Unknown version \"{0}\"; trying to parse it anyway.", m_BSPHeader.m_Version);

            //TODO: Implement version-check!

            //ParseAndCheckLumpData(str, eLumpIndex.LUMP_VERTEXES, out m_Vertexes, BSPFlags.MAX_SURFINFO_VERTS, "Vertexes");
            m_Vertexes = ParseLumpData<mvertex_t>(str, eLumpIndex.LUMP_VERTEXES);
            ParsePlanes(str);
            m_Edges = ParseLumpData<dedge_t>(str, eLumpIndex.LUMP_EDGES);
            m_Surfedges = ParseLumpData<int>(str, eLumpIndex.LUMP_SURFEDGES);
            m_Leaves = ParseLumpData<dleaf_t>(str, eLumpIndex.LUMP_LEAFS);
            ParseNodes(str);
            m_Surfaces = ParseLumpData<dface_t>(str, eLumpIndex.LUMP_FACES);
            m_OriginalFaces = ParseLumpData<dface_t>(str, eLumpIndex.LUMP_ORIGINALFACES);
            m_Texinfos = ParseLumpData<texinfo_t>(str, eLumpIndex.LUMP_TEXINFO);
            m_Brushes = ParseLumpData<dbrush_t>(str, eLumpIndex.LUMP_BRUSHES);
            m_Brushsides = ParseLumpData<dbrushside_t>(str, eLumpIndex.LUMP_BRUSHSIDES);
            //m_Leaffaces = ParseLumpData<ushort>(str, eLumpIndex.LUMP_LEAFFACES);
            //m_Leafbrushes = ParseLumpData<ushort>(str, eLumpIndex.LUMP_LEAFBRUSHES);
            ParseAndCheckLumpData<ushort>(str, eLumpIndex.LUMP_LEAFFACES, out m_Leaffaces, BSPFlags.MAX_MAP_LEAFFACES, "leaffaces");
            ParseAndCheckLumpData<ushort>(str, eLumpIndex.LUMP_LEAFBRUSHES, out m_Leafbrushes, BSPFlags.MAX_MAP_LEAFBRUSHES, "leafbrushes");

            //ParsePolygons();
            ParseEntities(str);
            ParseStaticProps(str);

            Program.Logger.Log("[BSP] Parsed successfully");
        }

        private void ParseStaticProps(Stream str)
        {
            var gameLump = m_BSPHeader.m_Lumps[(int)eLumpIndex.LUMP_GAME_LUMP];
            var gameLumpHeader = str.Read<dgamelumpheader_t>(gameLump.m_Fileofs);
            m_GameLumps = new dgamelump_t[gameLumpHeader.m_LumpCount];
            for (int i = 0; i < m_GameLumps.Length; i++)
                m_GameLumps[i] = str.Read<dgamelump_t>();

            //if (m_GameLumps.Any(x=>x.m_Id == 1936749168))
            //{
            var staticPropsGameLump = str.Read<StaticPropDictLump_t>(m_GameLumps.First(x => x.m_Id == 1936749168).m_FileOfs);
            //Read names
            m_StaticPropsModelNames = str.ReadArray<StaticPropDictLumpName>(staticPropsGameLump.m_DictEntries).Select(x => x.m_Name).ToArray();
            //Read leaves
            var staticPropLeafLumps = str.Read<StaticPropLeafLump_t>();
            var leaves = str.ReadArray<ushort>(staticPropLeafLumps.m_LeafEntries);
            var numProps = str.Read<int>();
            m_StaticProps = str.ReadArray<StaticPropLump_t>(numProps);

            //}
            //else { }

        }

        private void ParseEntities(Stream str)
        {
            var entitiesLump = m_BSPHeader.m_Lumps[(int)eLumpIndex.LUMP_ENTITIES];
            var data = new byte[entitiesLump.m_Filelen];
            str.Position = entitiesLump.m_Fileofs;
            str.Read(data, 0, data.Length);
            m_EntitiesASCII = Encoding.ASCII.GetString(data);
            File.WriteAllText("map_entities.txt", m_EntitiesASCII);
        }

        private void ParsePolygons()
        {
            List<Polygon> polygons = new List<Polygon>();
            foreach (var surface in m_Surfaces)
            {
                var first_edge = surface.m_Firstedge;
                var num_edges = surface.m_Numedges;

                if (num_edges < 3 || num_edges > BSPFlags.MAX_SURFINFO_VERTS)
                    continue;

                if (surface.m_Texinfo <= 0)
                    continue;

                Polygon poly = new Polygon();
                for (int i = 0; i < num_edges; i++)
                {
                    var edge_index = m_Surfedges[first_edge + 1];
                    if (edge_index >= 0)
                        poly.m_Verts[i] = m_Vertexes[m_Edges[edge_index].m_V[0]].m_Position;
                    else
                        poly.m_Verts[i] = m_Vertexes[m_Edges[-edge_index].m_V[1]].m_Position;
                }

                poly.m_nVerts = num_edges;
                poly.m_Plane = new VPlane(m_Planes[surface.m_Planenum].m_Normal, m_Planes[surface.m_Planenum].m_Distance);
                polygons.Add(poly);
            }
            m_Polygons = polygons.ToArray();
        }
        private void ParsePlanes(Stream str)
        {
            m_PlanesAddress = m_BSPHeader.m_Lumps[(int)eLumpIndex.LUMP_PLANES].m_Fileofs;
            var planes = ParseLumpData<dplane_t>(str, eLumpIndex.LUMP_PLANES);
            int plane_bits;
            m_Planes = new cplane_t[planes.Length];

            for (int i = 0; i < planes.Length; i++)
            {
                var op = new cplane_t();
                var ip = planes[i];

                plane_bits = 0;
                for (int j = 0; j < 3; j++)
                {
                    op.m_Normal[j] = ip.m_Normal[j];
                    if (op.m_Normal[j] < 0f)
                    {
                        plane_bits |= 1 << j;
                    }
                }

                op.m_Distance = ip.m_Distance;
                op.m_Type = ip.m_Type;
                op.m_SignBits = (byte)plane_bits;

                m_Planes[i] = op;
            }
        }
        private void ParseNodes(Stream str)
        {
            var nodes = ParseLumpData<dnode_t>(str, eLumpIndex.LUMP_NODES);
            m_Nodes = new snode_t[nodes.Length];

            for (int i = 0; i < m_Nodes.Length; i++)
            {
                var op = new snode_t(0);
                var ip = nodes[i];

                Array.Copy(ip.m_Mins, op.m_Mins, ip.m_Mins.Length);
                Array.Copy(ip.m_Maxs, op.m_Mins, ip.m_Maxs.Length);
                op.m_Planenum = ip.m_Planenum;
                op.m_pPlane = ip.m_Planenum;
                op.m_Firstface = ip.m_Firstface;
                op.m_Numfaces = ip.m_Numfaces;

                for (int j = 0; j < 2; j++)
                {
                    var child_index = ip.m_Children[j];
                    op.m_Children[j] = child_index;

                    if (child_index >= 0)
                    {
                        op.m_LeafChildren = 0;
                        op.m_NodeChildren = child_index;
                    }
                    else
                    {
                        op.m_LeafChildren = -(child_index + 1);
                        op.m_NodeChildren = 0;
                    }
                }

                m_Nodes[i] = op;
            }
        }
        private void ParseAndCheckLumpData<T>(Stream str, eLumpIndex index, out T[] data, int max, string name) where T : struct
        {
            data = ParseLumpData<T>(str, index);
            if (data.Length > max)
                throw new Exception(string.Format("[BSP] {0} has too many entries; Parsed more than required.", name));
            else if (data.Length == 0)
                throw new Exception(string.Format("[BSP] {0} has no entries.", name));
        }

        private T[] ParseLumpData<T>(Stream str, eLumpIndex index) where T : struct
        {
            var lump = m_BSPHeader.m_Lumps[(int)index];
            if (lump.m_FourCC != 0)
                throw new Exception("LZMA!");
            var count = lump.m_Filelen / SizeCache<T>.Size;
            if (count <= 0)
                return new T[0];

            return str.ReadArray<T>(lump.m_Fileofs, count);
        }
        #endregion

        #region VISCHECK
        private dleaf_t GetLeafForPoint(Vector3 point)
        {
            int node = 0;
            snode_t pNode;
            cplane_t pPlane;

            float d = 0.0f;
            while (node >= 0)
            {
                pNode = m_Nodes[node];
                pPlane = m_Planes[pNode.m_Planenum];

                d = point.Dot(pPlane.m_Normal) - pPlane.m_Distance;

                if (d > 0)
                    node = pNode.m_Children[0];
                else
                    node = pNode.m_Children[1];
            }

            return m_Leaves[-node - 1];
        }

        private static ContentsFlag LEAF_FLAGS = ContentsFlag.CONTENTS_SOLID | ContentsFlag.CONTENTS_DETAIL;

        public bool IsVisible(Vector3 start, Vector3 end)
        {
            var direction = end - start;
            var point = start;
            int stepCount = (int)direction.Length;

            direction *= 1f / (float)stepCount;

            var leaf = new dleaf_t();

            while (stepCount > 0)
            {
                point += direction;
                leaf = GetLeafForPoint(point);

                if (leaf.m_Area != -1)
                {
                    if ((leaf.m_Contents & LEAF_FLAGS) != 0)
                        break;
                }

                stepCount--;
            }

            return (leaf.m_Contents & ContentsFlag.CONTENTS_SOLID) == 0;
        }
        #endregion

    }
}
