using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoSceneLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    [Header("Video Settings")]
    public string videoFileName;
    public string nextSceneName;

    private void Start()
    {
        if (videoPlayer == null)
        {
            Debug.LogWarning("VideoPlayer component not assigned!");
            return;
        }

        string videoURL = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoURL;

        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("No next scene specified.");
        }
    }
}