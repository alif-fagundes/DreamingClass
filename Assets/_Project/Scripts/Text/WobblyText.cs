using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WobblyText : MonoBehaviour
{

    public TMP_Text textComponent;

    void Start()
    {
        
    }

    void Update()
    {
        textComponent.ForceMeshUpdate();
        var _textInfo = textComponent.textInfo;

        for (int i = 0; i < _textInfo.characterCount; ++i)
        {
            var _charInfo = _textInfo.characterInfo[i];

            if (!_charInfo.isVisible)
            {
                continue;
            }

            var _verts = _textInfo.meshInfo[_charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; ++j)
            {
                var _orig = _verts[_charInfo.vertexIndex + j];

                _verts[_charInfo.vertexIndex + j] = _orig + new Vector3(0, Mathf.Sin(Time.time * 2f + _orig.x * 0.01f) * 10f, 0);

            }
         }

         for (int i = 0; i < _textInfo.meshInfo.Length; ++i)
         {
            var _meshInfo = _textInfo.meshInfo[i];
            _meshInfo.mesh.vertices = _meshInfo.vertices;
            textComponent.UpdateGeometry(_meshInfo.mesh, i);
         }
        
    }
}
