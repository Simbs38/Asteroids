using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIIcon : MonoBehaviour
{
    public MeshFilter ShipMeshF;

    public void SetWireframe()
    {
        Mesh mesh = ShipMeshF.mesh;
        mesh.SetIndices(GetLines(mesh), MeshTopology.Lines, 0);
        ShipMeshF.mesh = mesh;
    }

    private int[] GetLines(Mesh mesh)
    {
        HashSet<Tuple<int, int>> lines = new HashSet<Tuple<int, int>>();

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            lines.Add(OrderLine(mesh.triangles[i], mesh.triangles[i + 1]));
            lines.Add(OrderLine(mesh.triangles[i + 1], mesh.triangles[i + 2]));
            lines.Add(OrderLine(mesh.triangles[i + 2], mesh.triangles[i]));
        }

        int[] ans = new int[lines.Count * 2];
        int index = 0;

        foreach (Tuple<int, int> item in lines)
        {
            ans[index++] = item.Item1;
            ans[index++] = item.Item2;
        }

        return ans;
    }

    private Tuple<int, int> OrderLine(int opA, int opB) => opA < opB ? new Tuple<int, int>(opA, opB) : new Tuple<int, int>(opB, opA);
}