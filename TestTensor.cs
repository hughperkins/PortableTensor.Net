using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class TestTensor : MonoBehaviour
{
    void Start()
    {
        Tensor<int> a = new Tensor<int>(new int[]{3, 5});
        Debug.Log($"a {a}");

        a[new int[]{2, 1}] = 5;
        a[new int[]{2, 2}] = 4;

        Debug.Log($"a {a}");

        a.Transpose(new int[]{0, 1});
        Debug.Log($"a {a}");

        Tensor<int> b = a.Transpose(new int[]{1, 0});
        Debug.Log($"a {a}");
        Debug.Log($"b {b}");

        b[new int[]{2, 1}] = 14;
        Debug.Log($"a {a}");
        Debug.Log($"b {b}");

        var c = b.Slice(1, 1, 2);
        Debug.Log($"a {a}");
        Debug.Log($"b {b}");
        Debug.Log($"c {c}");

        c = b.Slice(0, 1, 3);
        Debug.Log($"c {c}");

        c = c.Slice(1, 1, 3);
        Debug.Log($"c {c}");

        c[new int[]{0, 1}] = 51;
        Debug.Log($"a {a}");
        Debug.Log($"b {b}");
        Debug.Log($"c {c}");
    }
}
