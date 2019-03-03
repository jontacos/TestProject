using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Jontacos
{
    public static class Utils
    {
        /// <summary>
        /// IOを利用した直接的なファイルロード(Androidでは不可)
        /// </summary>
        /// <param name="path"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Texture2D LoadTextureByFileIO(string path, int width, int height)
        {
            var tex = new Texture2D(width, height);
            byte[] b = File.ReadAllBytes(path);
            tex.LoadImage(b, false);
            return tex;
        }
        /// <summary>
        /// UnityWebRequestを利用したファイルロード
        /// </summary>
        /// <param name="path"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Texture2D LoadTextureByWebRequest(string path, int width, int height)
        {
            var tex = new Texture2D(width, height);
            var enumerator = LoadTextureByWebRequest(path, 100, 100, tex);
            while (enumerator.MoveNext()) { Debug.Log(Time.realtimeSinceStartup); }
            return tex;
        }
        private static IEnumerator LoadTextureByWebRequest(string path, int width, int height, Texture2D texture)
        {
            var request = UnityWebRequest.Get(path);
            yield return request.SendWebRequest();

            while(!request.isDone)
                yield return null;

            byte[] b = request.downloadHandler.data;
            texture.LoadImage(b, false);
        }

        /// <summary>
        /// Ease-In。最初ゆっくり、後半速い。
        /// </summary>
        /// <param name="start">開始値</param>
        /// <param name="end">終了値</param>
        /// <param name="elapsed">Ease経過時間</param>
        /// <param name="time">Easeさせる時間</param>
        /// <returns></returns> 
        public static float EaseIn(float start, float end, float elapsed, float time)
        {
            elapsed /= time;
            var dif = end - start;
            return dif * elapsed * elapsed + start;
        }
        public static Vector2 EaseIn(Vector2 start, Vector2 end, float elapsed, float time)
        {
            elapsed /= time;
            var dif = end - start;
            return dif * elapsed * elapsed + start;
        }

        /// <summary>
        /// Ease-Out。最初速い、後半ゆっくり。
        /// </summary>
        /// <param name="start">開始値</param>
        /// <param name="end">終了値</param>
        /// <param name="elapsed">Ease経過時間</param>
        /// <param name="time">Easeさせる時間</param>
        /// <returns></returns> 
        public static float EaseOut(float start, float end, float elapsed, float time)
        {
            elapsed /= time;
            var dif = end - start;
            return -dif * Mathf.Pow(elapsed - 1f, 2) + end;
        }
        public static Vector2 EaseOut(Vector2 start, Vector2 end, float elapsed, float time)
        {
            elapsed /= time;
            var dif = end - start;
            var a = -dif * Mathf.Pow(elapsed - 1f, 2);
            return -dif * Mathf.Pow(elapsed - 1f, 2) + end;
        }

        /// <summary>
        /// Ease-InOut。最初ゆっくり、最後もゆっくり。
        /// </summary>
        /// <param name="start">開始値</param>
        /// <param name="end">終了値</param>
        /// <param name="elapsed">Ease経過時間</param>
        /// <param name="time">Easeさせる時間</param>
        /// <returns></returns> 
        public static float EaseInOut(float start, float end, float elapsed, float time)
        {
            var result = 0f;
            var half = time * 0.5f;
            var dif = end - start;
            if (elapsed / time < 0.5f)
                result = EaseIn(start, start + dif * 0.5f, elapsed, half);
            else
                result = EaseOut(start + dif * 0.5f, end, elapsed - half, half);
            return result;
        }
        public static Vector2 EaseInOut(Vector2 start, Vector2 end, float elapsed, float time)
        {
            var result = Vector2.zero;
            var half = time * 0.5f;
            var dif = end - start;
            if (elapsed / time < 0.5f)
                result = EaseIn(start, start + dif * 0.5f, elapsed, half);
            else
                result = EaseOut(start + dif * 0.5f, end, elapsed - half, half);
            return result;
        }
    }
}