using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MedaiPlayerSampleGUI : MonoBehaviour
{

    public MediaPlayerCtrl skyMediaPlayerController;
    public Transform CurrentTime, DrationTime;
    public Transform progressBar;
    public GameObject Screen_L, Screen_R, VR_L, VR_R;
    private float currVolume;
    // Use this for initialization
    void Start()
    {
        skyMediaPlayerController.OnEnd += OnEnd;
    }
    // Update is called once per frame
    void Update()
    {
        //Just a Demo,  Need to Modify
        int currTime = skyMediaPlayerController.GetSeekPosition() / 1000;
        int currH = currTime / 3600;
        int currM = (currTime - currH * 3600) / 60;
        int currS = currTime - currH * 3600 - currM * 60;
        CurrentTime.GetComponent<Text>().text = string.Format("{0:D2}:{1:D2}:{2:D2}", currH, currM, currS);

        int overallTime = skyMediaPlayerController.GetDuration() / 1000;
        int overallH = overallTime / 3600;
        int overallM = (overallTime - overallH * 3600) / 60;
        int overallS = overallTime - overallH * 3600 - overallM * 60;
        DrationTime.GetComponent<Text>().text = string.Format("{0:D2}:{1:D2}:{2:D2}", overallH, overallM, overallS);

        progressBar.GetComponent<Slider>().value = currTime;
        progressBar.GetComponent<Slider>().maxValue = overallTime;
    }
    public void loadMedia(string path)
    {
        loadVideo(path);
    }
    IEnumerator loadImage(string path)
    {
        WWW www = new WWW("file://" + path);
        yield return www.isDone;
        Texture2D t2d = www.texture;
        skyMediaPlayerController.GetComponent<Renderer>().material.mainTexture = t2d;
    }
    public void loadVideo(string url)
    {
        skyMediaPlayerController.Load("file://" + url);
    }

    public void stopMedia()
    {
        skyMediaPlayerController.Stop();
        skyMediaPlayerController.UnLoad();
        skyMediaPlayerController.GetComponent<Renderer>().material.mainTexture = (Texture2D)Resources.Load("Dummy");
    }
    public void play_pause()
    {
        if( MediaPlayerCtrl.m_CurrentState == MediaPlayerCtrl.MEDIAPLAYER_STATE.PLAYING )
        {
            skyMediaPlayerController.Pause();
        }
        else
        {
            skyMediaPlayerController.Play(); 
        }
    }
    public void stop()
    {
        skyMediaPlayerController.Stop();
    }
    public void unload()
    {
        skyMediaPlayerController.UnLoad();
    }
    public void SeekForward()
    {
        int currTime = skyMediaPlayerController.GetSeekPosition();
        int overallTime = skyMediaPlayerController.GetDuration();
        int seek = currTime + overallTime / 100;
        if( (overallTime / 100) < 5000 )
        {
            seek = currTime + 5000;
        }
        else
        {
            seek = currTime + overallTime / 100;
        }
        if( seek > overallTime )
        {
            seek = overallTime;
        }
        skyMediaPlayerController.SeekTo(seek);
    }
    public void SeekBack()
    {
        int currTime = skyMediaPlayerController.GetSeekPosition();
        int overallTime = skyMediaPlayerController.GetDuration();
        int seek = currTime - overallTime / 100;
        if( (overallTime / 100) < 5000 )
        {
            seek = currTime - 5000;
        }
        else
        {
            seek = currTime - overallTime / 100;
        }
        if( seek <= 0 )
        {
            seek = 0;
        }
        skyMediaPlayerController.SeekTo(seek);
    }
    public void volUp()
    {
        MediaPlayerCtrl.voltest += 1f;
        if( MediaPlayerCtrl.voltest > 15 )
        {
            MediaPlayerCtrl.voltest = 15;
        }
        skyMediaPlayerController.SetVolume(MediaPlayerCtrl.voltest);

    }
    public void volDown()
    {
        MediaPlayerCtrl.voltest -= 1f;
        if( MediaPlayerCtrl.voltest < 0 )
        {
            MediaPlayerCtrl.voltest = 0;
        }
        skyMediaPlayerController.SetVolume(MediaPlayerCtrl.voltest);
    }
    public void LoadInternet()
    {
        skyMediaPlayerController.Stop();
        skyMediaPlayerController.UnLoad();
        skyMediaPlayerController.Load("http://www.lbj-ironman.com/3D_MV.mp4");
    }
    public void LoadNative()
    {
        skyMediaPlayerController.Stop();
        skyMediaPlayerController.UnLoad();
        skyMediaPlayerController.Load("file://mnt/sdcard/Movies/3D_MV.mp4");
    }


    void OnEnd()
    {
        //m_bFinish = true;
    }

    public void _2D_Mode()
    {

        Screen_L.SetActive(true);
        skyMediaPlayerController.m_TargetMaterial = Screen_L;
        skyMediaPlayerController.m_TargetMaterial_2 = null;
        Screen_R.SetActive(false);
        VR_L.SetActive(false);
        VR_R.SetActive(false);
        Screen_L.layer = 0;
        Screen_L.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1f, 1f);
        Screen_L.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f, 0f);
    }
    public void _3D_Mode()
    {
        Screen_L.SetActive(true);
        Screen_R.SetActive(true);
        skyMediaPlayerController.m_TargetMaterial = Screen_L;
        skyMediaPlayerController.m_TargetMaterial_2 = Screen_R;
        skyMediaPlayerController.m_TargetMaterial_2.SetActive(true);
        VR_L.SetActive(false);
        VR_R.SetActive(false);
        Screen_L.layer = 9;
        Screen_R.layer = 10;
        Screen_L.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f, 0f);
        Screen_L.GetComponent<Renderer>().material.mainTextureScale = new Vector2(0.5f, 1f);
        Screen_R.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.5f, 0f);
        Screen_R.GetComponent<Renderer>().material.mainTextureScale = new Vector2(0.5f, 1f);
    }

    public void Panorama_Mode()
    {
        VR_L.SetActive(true);
        skyMediaPlayerController.m_TargetMaterial = VR_L;
        skyMediaPlayerController.m_TargetMaterial_2 = null;
        VR_R.SetActive(false);
        VR_L.transform.localScale = new Vector3(800, 800, 800);
        VR_L.layer = 0;
        VR_L.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1f, 1f);
        VR_L.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f, 0f);
        Screen_L.SetActive(false);
        Screen_R.SetActive(false);

    }

    public void Panorama_3D_Mode()
    {
        VR_L.SetActive(true);
        VR_R.SetActive(true);
        VR_L.transform.localScale = new Vector3(800, 800, 800);
        VR_R.transform.localScale = new Vector3(800, 800, 800);
        VR_L.layer = 9;
        VR_R.layer = 10;
        skyMediaPlayerController.m_TargetMaterial_2 = VR_R;
        skyMediaPlayerController.m_TargetMaterial = VR_L;
        skyMediaPlayerController.m_TargetMaterial_2.SetActive(true);
        Screen_L.SetActive(false);
        Screen_R.SetActive(false);
        VR_L.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f, 0f);
        VR_L.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1f, 0.5f);
        VR_R.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0f, 0.5f);
        VR_R.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, 0.5f);
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene("Demo");
    }


}
