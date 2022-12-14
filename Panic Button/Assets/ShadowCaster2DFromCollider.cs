using System.Reflection;




using UnityEngine;




using UnityEngine.Experimental.Rendering.Universal;










/// <summary>




/// unitytips: ShadowCaster2DFromCollider Component




/// http://diegogiacomelli.com.br/unitytips-shadowcaster2-from-collider-component




/// <remarks>




/// Based on https://forum.unity.com/threads/can-2d-shadow-caster-use-current-sprite-silhouette.861256/




/// </remarks>




/// </summary>




[RequireComponent(typeof(ShadowCaster2D))]




[DefaultExecutionOrder(100)]




public class ShadowCaster2DFromCollider : MonoBehaviour




{










    static readonly FieldInfo _meshField;




    static readonly FieldInfo _shapePathField;




    static readonly MethodInfo _generateShadowMeshMethod;










    ShadowCaster2D _shadowCaster;










    EdgeCollider2D _edgeCollider;




    PolygonCollider2D _polygonCollider;










    static ShadowCaster2DFromCollider()




    {




        _meshField = typeof(ShadowCaster2D).GetField("m_Mesh", BindingFlags.NonPublic | BindingFlags.Instance);




        _shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);










        _generateShadowMeshMethod = typeof(ShadowCaster2D)




                                    .Assembly




                                    .GetType("UnityEngine.Experimental.Rendering.Universal.ShadowUtility")




                                    .GetMethod("GenerateShadowMesh", BindingFlags.Public | BindingFlags.Static);




    }










    private void Start()




    {




        _shadowCaster = GetComponent<ShadowCaster2D>();




        _edgeCollider = GetComponent<EdgeCollider2D>();










        if (_edgeCollider == null)




            _polygonCollider = GetComponent<PolygonCollider2D>();










        UpdateShadow();




    }










    public void UpdateShadow()




    {




        var points = _polygonCollider == null




            ? _edgeCollider.points




            : _polygonCollider.points;










        _shapePathField.SetValue(_shadowCaster, ConvertArray(points));




        _meshField.SetValue(_shadowCaster, new Mesh());




        _generateShadowMeshMethod.Invoke(_shadowCaster, new object[] { _meshField.GetValue(_shadowCaster), _shapePathField.GetValue(_shadowCaster) });




    }

    Vector3[] ConvertArray(Vector2[] v2)
    {
        Vector3[] v3 = new Vector3[v2.Length];
        for (int i = 0; i < v2.Length; i++)
        {
            Vector2 tempV2 = v2[i];
            v3[i] = new Vector3(tempV2.x, tempV2.y,0);
        }
        return v3;
    }


}