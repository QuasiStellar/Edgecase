// https://gist.github.com/mandarinx/ed733369fbb2eea6c7fa9e3da65a0e17

using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MeshFilter))]
    public class NormalsVisualizer : UnityEditor.Editor {

        private const string     EditorPrefKey = "_normals_length";
        private       Mesh       _mesh;
        private       MeshFilter _mf;
        private       Vector3[]  _verts;
        private       Vector3[]  _normals;
        private       float      _normalsLength = 1f;

        private void OnEnable() {
            _mf   = target as MeshFilter;
            if (_mf != null) {
                _mesh = _mf.sharedMesh;
            }
            _normalsLength = EditorPrefs.GetFloat(EditorPrefKey);
        }

        private void OnSceneGUI() {
            if (_mesh == null) {
                return;
            }

            Handles.matrix = _mf.transform.localToWorldMatrix;
            Handles.color = Color.yellow;
            _verts = _mesh.vertices;
            _normals = _mesh.normals;
            var len = _mesh.vertexCount;
        
            for (var i = 0; i < len; i++) {
                Handles.DrawLine(_verts[i], _verts[i] + _normals[i] * _normalsLength);
            }
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            EditorGUI.BeginChangeCheck();
            _normalsLength = EditorGUILayout.FloatField("Normals length", _normalsLength);
            if (EditorGUI.EndChangeCheck()) {
                EditorPrefs.SetFloat(EditorPrefKey, _normalsLength);
            }
        }
    }
}