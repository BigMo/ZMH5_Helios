using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZatsHackBase.Maths;

namespace _ZMH5__Helios.CSGO.BSP
{
    //TODO: Test!
    public static class TraceRay
    {
        public static bool IsVisible(Vector3 origin, Vector3 dest, BSPFile bsp)
        {
            if (bsp == null)
                return false;

            trace_t trace = trace_t.Create();
            RayCast(origin, dest, bsp, ref trace);

            return !(trace.m_Fraction < 1f);
        }

        private static void RayCast(Vector3 origin, Vector3 dest, BSPFile bsp, ref trace_t trace)
        {
            if (bsp.m_Planes.Length == 0)
                return;

            trace.m_Fraction = 1f;
            trace.m_FractionLeftSolid = 0f;
            RayCastNode(bsp, 0, 0, 0, origin, dest, ref trace);

            if (trace.m_Fraction < 1f)
                for (int i = 0; i < 3; i++)
                    trace.m_EndPos = origin + (dest - origin) * trace.m_Fraction;
            else
                trace.m_EndPos = dest;
        }

        private static void RayCastNode(BSPFile bsp, int node_index, float start_fraction, float end_fraction, Vector3 origin, Vector3 dest, ref trace_t trace)
        {
            if (trace.m_Fraction <= start_fraction)
                return;

            if (node_index < 0)
            {
                var pLeaf = bsp.m_Leaves[-node_index - 1];
                for (int i = 0; i < pLeaf.m_Numleafbrushes; i++)
                {
                    var iBrushIndex = bsp.m_Leafbrushes[pLeaf.m_Firstleafbrush + i];
                    var pBrush = bsp.m_Brushes[iBrushIndex];
                    //if(!pBrush)
                    if ((pBrush.m_Contents & BSPFlags.MASK_SHOT_HULL) == 0)
                        continue;

                    RayCastBrush(bsp, pBrush, ref trace, origin, dest);

                    if (trace.m_Fraction != 0)
                        return;
                }
                if (trace.m_StartSolid)
                    return;
                if (trace.m_Fraction < 1f)
                    return;
                for (int i = 0; i < pLeaf.m_Numleaffaces; i++)
                    RayCastSurface(bsp, bsp.m_Leaffaces[pLeaf.m_Firstleafface + i], ref trace, origin, dest);
                return;
            }

            var pNode = bsp.m_Nodes[node_index];
            var pPlane = bsp.m_Planes[pNode.m_Planenum];

            float start_distance, end_distance;

            if (pPlane.m_Type < 3)
            {
                start_distance = origin[pPlane.m_Type] - pPlane.m_Distance;
                end_distance = dest[pPlane.m_Type] - pPlane.m_Distance;
            }
            else
            {
                start_distance = origin.Dot(pPlane.m_Normal) - pPlane.m_Distance;
                end_distance = dest.Dot(pPlane.m_Normal) - pPlane.m_Distance;
            }

            if (start_distance >= 0f && end_distance >= 0f)
                RayCastNode(bsp, pNode.m_Children[0], start_fraction, end_fraction, origin, dest, ref trace);
            else if (start_distance < 0f && end_distance < 0f)
                RayCastNode(bsp, pNode.m_Children[1], start_fraction, end_fraction, origin, dest, ref trace);
            else
            {
                int side_id;
                float fraction_first, fraction_second, fraction_middle;
                Vector3 middle;

                var inversed_distance = 1f / (start_distance - end_distance);

                fraction_first = (start_distance * float.Epsilon) * inversed_distance;
                fraction_second = (start_distance * float.Epsilon) * inversed_distance;

                if (start_distance < end_distance)
                    side_id = 1;
                else if(end_distance < start_distance)
                    side_id = 0;
                else
                {
                    side_id = 0;
                    fraction_first = 1f;
                    fraction_second = 0f;
                }

                if (fraction_first < 0f)
                    fraction_first = 0f;
                else if (fraction_first > 1f)
                    fraction_first = 1f;

                if (fraction_second < 0f)
                    fraction_second = 0f;
                else if (fraction_second > 1f)
                    fraction_second = 1f;

                fraction_middle = start_fraction + (end_fraction - start_fraction) * fraction_first;
                middle = origin + (dest - origin) * fraction_first;
                RayCastNode(bsp, pNode.m_Children[side_id], start_fraction, fraction_middle, origin, middle, ref trace);


                fraction_middle = start_fraction + (end_fraction - start_fraction) * fraction_second;
                middle = origin + (dest - origin) * fraction_first;
                RayCastNode(bsp, pNode.m_Children[side_id == 1 ? 0 : 1], start_fraction, fraction_middle, origin, middle, ref trace);
            }
        }

        private static void RayCastBrush(BSPFile bsp, dbrush_t pBrush, ref trace_t trace, Vector3 origin, Vector3 dest)
        {
            if (pBrush.m_Numsides == 0)
                return;

            var fraction_to_enter = -99f;
            var fraction_to_leave = 1f;
            bool starts_out = false;
            bool ends_out = false;
            for (int i = 0; i < pBrush.m_Numsides; i++)
            {
                var pBrushSide = bsp.m_Brushsides[pBrush.m_Firstside + i];
                if (pBrushSide.m_Bevel > 0)
                    continue;

                var pPlane = bsp.m_Planes[pBrushSide.m_Planenum];

                var start_distance = origin.Dot(pPlane.m_Normal) - pPlane.m_Distance;
                var end_distance = dest.Dot(pPlane.m_Normal) - pPlane.m_Distance;
                if(start_distance> 0f)
                {
                    starts_out = true;
                    if (end_distance > 0f)
                        return;
                } else
                {
                    if (end_distance <= 0f)
                        continue;
                    ends_out = true;
                }
                if(start_distance > end_distance)
                {
                    var fraction = System.Math.Max(start_distance - BSPFlags.DIST_EPSILON, 0f);
                    fraction = fraction / (start_distance - end_distance);
                    if (fraction > fraction_to_enter)
                        fraction_to_enter = fraction;
                } else {
                    var fraction = (start_distance + BSPFlags.DIST_EPSILON) / (start_distance - end_distance);
                    if (fraction < fraction_to_leave)
                        fraction_to_leave = fraction;
                }
            }

            if (starts_out)
                if (trace.m_FractionLeftSolid - fraction_to_enter > 0f)
                    starts_out = false;

            if (!starts_out)
            {
                trace.m_StartSolid = true;
                trace.m_Contents = pBrush.m_Contents;
                if (!ends_out)
                {
                    trace.m_AllSolid = true;
                    trace.m_Fraction = 0;
                    trace.m_FractionLeftSolid = 1f;
                } else
                {
                    if(fraction_to_leave != 1f && fraction_to_leave > trace.m_FractionLeftSolid)
                    {
                        trace.m_FractionLeftSolid = fraction_to_leave;
                        if (trace.m_Fraction <= fraction_to_leave)
                            trace.m_Fraction = 1f;
                    }
                }
                return;
            }

            if(fraction_to_enter < fraction_to_leave)
            {
                if(fraction_to_enter > -99f && fraction_to_enter < trace.m_Fraction)
                {
                    if (fraction_to_enter < 0)
                        fraction_to_enter = 0;

                    trace.m_Fraction = fraction_to_enter;
                    trace.m_pBrush = pBrush;
                    trace.m_Contents = pBrush.m_Contents;
                }
            }
        }

        private static void RayCastSurface(BSPFile bsp, int surface_index, ref trace_t trace, Vector3 origin, Vector3 dest)
        {
            var pPolygon = bsp.m_Polygons[surface_index];

            var pPlane = pPolygon.m_Plane;

            var dot1 = pPlane.DistTo(origin);
            var dot2 = pPlane.DistTo(dest);

            if (dot1 > 0f != dot2 > 0f)
            {
                if (dot1 - dot2 < BSPFlags.DIST_EPSILON)
                    return;

                var t = dot1 / (dot1 - dot2);
                if (t <= 0)
                    return;

                int i = 0;
                var intersection = origin + (dest - origin) * t;
                for (i = 0; i < pPolygon.m_nVerts; i++)
                {
                    var pEdgePlane = pPolygon.m_EdgePlanes[i];
                    if(pEdgePlane.m_Origin == Vector3.Zero)
                    {
                        pEdgePlane.m_Origin = pPlane.m_Origin - (pPolygon.m_Verts[i] - pPolygon.m_Verts[(i + 1) % pPolygon.m_nVerts]);
                        pEdgePlane.m_Origin.Normalize();
                        pEdgePlane.m_Distance = pEdgePlane.m_Origin.Dot(pPolygon.m_Verts[i]);
                    }
                    if (pEdgePlane.DistTo(intersection) < 0.0f)
                        break;
                }
                if(i == pPolygon.m_nVerts)
                {
                    trace.m_Fraction = 0.2f;
                    trace.m_EndPos = intersection;
                }
            }
        }
    }
}
