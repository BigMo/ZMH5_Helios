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
        private dheader_t m_BSPHeader;
        private mvertex_t[] m_Vertexes;
        public cplane_t[] m_Planes;
        private dedge_t[] m_Edges;
        private int[] m_Surfedges;
        public dleaf_t[] m_Leaves;
        public snode_t[] m_Nodes;
        private dface_t[] m_Surfaces;
        private texinfo_t[] m_Texinfos;
        public dbrush_t[] m_Brushes;
        public dbrushside_t[] m_Brushsides;
        public ushort[] m_Leaffaces;
        public ushort[] m_Leafbrushes;
        public Polygon[] m_Polygons;
        private long m_PlanesAddress;
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
            m_Texinfos = ParseLumpData<texinfo_t>(str, eLumpIndex.LUMP_TEXINFO);
            m_Brushes = ParseLumpData<dbrush_t>(str, eLumpIndex.LUMP_BRUSHES);
            m_Brushsides = ParseLumpData<dbrushside_t>(str, eLumpIndex.LUMP_BRUSHSIDES);
            ParseAndCheckLumpData<ushort>(str, eLumpIndex.LUMP_LEAFFACES, out m_Leaffaces, BSPFlags.MAX_MAP_LEAFBRUSHES, "leaffaces");
            ParseAndCheckLumpData<ushort>(str, eLumpIndex.LUMP_LEAFBRUSHES, out m_Leafbrushes, BSPFlags.MAX_MAP_LEAFBRUSHES, "leafbrushes");
            ParsePolygons();

            Program.Logger.Log("[BSP] Parsed successfully");
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
                for(int i = 0; i< num_edges; i++)
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

            for(int i = 0; i < planes.Length; i++)
            {
                var op = new cplane_t();
                var ip = planes[i];

                plane_bits = 0;
                for(int j = 0; j < 3; j++)
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
                var op = new snode_t();
                var ip = nodes[i];

                Array.Copy(ip.m_Mins, op.m_Mins, ip.m_Mins.Length);
                Array.Copy(ip.m_Maxs, op.m_Mins, ip.m_Maxs.Length);
                op.m_Planenum = ip.m_Planenum;
                op.m_pPlane = ip.m_Planenum;
                op.m_Firstface = ip.m_Firstface;
                op.m_Numfaces = ip.m_Numfaces;

                for(int j = 0; j < 2; j++)
                {
                    var child_index = ip.m_Children[j];
                    op.m_Children[j] = child_index;

                    if(child_index >= 0)
                    {
                        op.m_LeafChildren = 0;
                        op.m_NodeChildren = child_index;
                    } else
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
            var count = lump.m_Filelen / Marshal.SizeOf(typeof(T));
            if (count <= 0)
                return new T[0];

            return str.ReadArray<T>(lump.m_Fileofs, count);
        }
        #endregion


    }
}
