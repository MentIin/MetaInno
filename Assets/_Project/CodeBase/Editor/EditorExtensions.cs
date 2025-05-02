using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Editor
{
    public static class EditorExtensions
    {
        public static T LoadInEditor<T>(
            this AssetReference assetReference
        ) where T : UnityEngine.Object {
            var path = AssetDatabase.GUIDToAssetPath(assetReference.RuntimeKey.ToString());
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
        public static void SetTextureImporterFormat( Texture2D texture, bool isReadable)
        {
            if ( null == texture ) return;

            string assetPath = AssetDatabase.GetAssetPath( texture );
            var tImporter = AssetImporter.GetAtPath( assetPath ) as TextureImporter;
            if ( tImporter != null )
            {
                //tImporter.textureType = TextureImporterType.Advanced;

                tImporter.isReadable = isReadable;

                AssetDatabase.ImportAsset( assetPath );
                AssetDatabase.Refresh();
            }
        }
    }
}