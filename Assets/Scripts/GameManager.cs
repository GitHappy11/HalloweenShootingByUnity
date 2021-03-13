
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool isPaused = false;
    public Image panelMenu;
    public Toggle toggle;

    private void Update()
    {

        if (panelMenu.gameObject.activeSelf == false)
        {
            Time.timeScale = 1;
            isPaused = false;
            Cursor.visible = false;   //隐藏光标
        }
        else
        {
            Time.timeScale = 0;
            isPaused = true;
            Cursor.visible = true;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelMenu.gameObject.activeSelf == false)
            {
                panelMenu.gameObject.SetActive(true);
            }
            else
            {
                panelMenu.gameObject.SetActive(false);
            }
        }


        //静音 保存机制不建议在Update中使用，这里只是示范，请规范化使用【本项目其他类似操作同理】
        if (toggle.isOn==true)
        {
            GetComponent<AudioSource>().mute = false;
            PlayerPrefs.SetInt("mute", 0);
        }
        else
        {
            GetComponent<AudioSource>().mute = true;
            PlayerPrefs.SetInt("mute", 1);
        }

        
        
    }

    private void Start()
    {
        //先检查这个数据是否存在
        if (PlayerPrefs.HasKey("mute"))
        {
            //然后就可以进行加载
            if (PlayerPrefs.GetInt("mute") == 1)
            {
                toggle.isOn = false;
            }
            else
            {
                toggle.isOn = true;
            }
        }
        
    }
}
