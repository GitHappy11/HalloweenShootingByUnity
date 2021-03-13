using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using LitJson;
using System.Xml;

//简单的游戏数据保存系统
//依托于类保存
//提供了三种保存方式 二进制保存，XML保存，Json保存【需要相应的dll】
public class Save 
{

    readonly string savePathByBin = Application.dataPath+"/GameData/Bin.txt";
    readonly string savePathByJson = Application.dataPath+"/GameData/Json.txt";
    readonly string savePathByXML = Application.dataPath+"/GameData/XML.txt";

    private static Save instance=null;
    public  static Save Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Save();
            }
            return instance;
        }


        set
        {

        }
    }

    public  void SavaGame(SaveType saveType)
    {
        switch (saveType)
        {
            case SaveType.Bin:
                SaveByBin();
                break;
            case SaveType.XML:
                SaveByXML();
                break;
            case SaveType.Json:
                SaveByJson();
                break;
            default:
                break;
        }
    }

    public void LoadGame(SaveType saveType)
    {
        switch (saveType)
        {
            case SaveType.Bin:
                LoadByBin();
                break;
            case SaveType.XML:
                LoadByXML();
                break;
            case SaveType.Json:
                LoadByJson();
                break;
            default:
                break;
        }
    }


    #region 二进制数据存储方式
    //二进制保存方式
    private void SaveByBin()
    {
        //获得一个需要保存数据的类 我这里就把LocalData类里的数据保存下来
        //注意：这个类是要标记可序列化的[Serializable],获得这个SaveData类的方式最好是用一个方法初始化一下【其他保存方式也类似】
        SaveData saveData = new SaveData();
        saveData.shootNum = LocalData.shootNum;
        saveData.score = LocalData.score;
        //创建一个二进制格式化程序
        BinaryFormatter bf = new BinaryFormatter();
        //创建一个文件流 后面文件保存的格式什么都可以，可以创建自己的专属格式【这里使用txt可以更直接看到里面写了什么】
        FileStream fileStream = File.Create(savePathByBin);
        //用二进制格式化程序的序列化方法来序列化Save对象
        //参数：创建的文件流，和序列化的对象
        bf.Serialize(fileStream, saveData);
        //关闭流  这里可以使用Using方式，可能会更简单
        fileStream.Close();
        //如果文件存在，说明保存成功【后续保存可能需要对比文件差异】
        if (File.Exists(savePathByBin))
        {
            Debug.Log("保存成功！");
        }
    
    }
    //二进制加载方式
    private void LoadByBin()
    {
        //反序列化过程
        //创建一个二进制格式化程序
        BinaryFormatter bf = new BinaryFormatter();
        //先看文件是否存在
        if (File.Exists(savePathByBin))
        {
            //打开一个文件流 参数：文件路径，文件操作模式【打开】
            FileStream fileStream = File.Open(savePathByBin, FileMode.Open);
            //调用格式化程序的反序列化方法，讲文件流转化为一个Save对象
            SaveData saveData = (SaveData)bf.Deserialize(fileStream);
            //关闭文件夹流【一样的，可以使用using】
            fileStream.Close();
            //后面就是把取出来的这个类的数据附加到游戏内了
            LocalData.score = saveData.score;
            LocalData.shootNum = saveData.shootNum;
            Debug.Log("加载成功");
        }


    }
    #endregion

    #region JSON数据存储方式
    //JSON方式存储数据
    private void SaveByJson()
    {
        //获得一个需要保存数据的类 我这里就把LocalData类里的数据保存下来
        //注意：这个类是要标记可序列化的[Serializable],获得这个SaveData类的方式最好是用一个方法初始化一下【其他保存方式也类似】
        SaveData saveData = new SaveData();
        saveData.shootNum = LocalData.shootNum;
        saveData.score = LocalData.score;
        //利用JsonManager将Save对象转化为Json格式的字符串
        string saveJsonStr = JsonMapper.ToJson(saveData);
        //创建一个StreamWrite，并将字符串写入文件中
        StreamWriter sw = new StreamWriter(savePathByJson);
        sw.Write(saveJsonStr);
        //关闭StreamWrite【同理，可以使用Using
        sw.Close();

        //如果文件存在，说明保存成功【后续保存可能需要对比文件差异】
        if (File.Exists(savePathByJson))
        {
            Debug.Log("保存成功！");
        }

    }
    //JSON的方式加载数据
    private void LoadByJson()
    {
        //先看文件是否存在
        if (File.Exists(savePathByJson))
        {
            //创建一个StreamReader，用来读取流
            StreamReader sr = new StreamReader(savePathByJson);
            //将读取到的流赋值给jsonStr
            string jsonStr = sr.ReadToEnd();
            //关闭读取流【可以使用Using】
            sr.Close();
            //将字符串JsonStr转换为Save对象
            SaveData saveData = JsonMapper.ToObject<SaveData>(jsonStr);
            //后面就是把数据读取出来了
            if (saveData!=null)
            {
                LocalData.score =saveData.score ;
                LocalData.shootNum = saveData.shootNum;
                Debug.Log("加载成功！");
            }
            else
            {
                Debug.LogWarning("数据读取失败！");
            }
            
            
        }
        else
        {
            Debug.LogWarning("存档文件不存在!");
        }
    }
    #endregion

    #region XML数据存储方式
    //XML存储
    private void SaveByXML()
    {
        //创建XML文件的存储路径
        XmlDocument xmlDoc = new XmlDocument();
        //创建根节点，即最上层节点
        XmlElement root = xmlDoc.CreateElement("save");
        //设置根节点中的值
        root.SetAttribute("name", "saveFile1");
        //遍历save中存储的数据，将数据转换成XML格式 列表专用存储方式，此次实例没有，可做参考
        #region 列表数据存储
        //创建XmlElement
        //XmlElement target;
        //XmlElement targetPosition;
        //XmlElement monsterType;
        //for (int i = 0; i < save.livingTargetPositions.Count; i++)
        //{
        //    target = xmlDoc.CreateElement("target");
        //    targetPosition = xmlDoc.CreateElement("targetPosition");
        //    //设置InnerText值
        //    targetPosition.InnerText = save.livingTargetPositions[i].ToString();
        //    monsterType = xmlDoc.CreateElement("monsterType");
        //    monsterType.InnerText = save.livingMonsterTypes[i].ToString();

        //    //设置节点间的层级关系 root -- target -- (targetPosition, monsterType)
        //    target.AppendChild(targetPosition);
        //    target.AppendChild(monsterType);
        //    root.AppendChild(target);
        //}
        #endregion

        //设置射击数和分数节点并设置层级关系  xmlDoc -- root --(target-- (targetPosition, monsterType), shootNum, score)
        XmlElement shootNum = xmlDoc.CreateElement("shootNum");
        shootNum.InnerText = LocalData.shootNum.ToString();
        root.AppendChild(shootNum);

        XmlElement score = xmlDoc.CreateElement("score");
        score.InnerText = LocalData.score.ToString();
        root.AppendChild(score);

        xmlDoc.AppendChild(root);
        xmlDoc.Save(savePathByXML);

        if (File.Exists(savePathByXML))
        {
            Debug.Log("保存成功！");
        }
    }
    //XML读取
    private void LoadByXML()
    {

        if (File.Exists(savePathByXML))
        {
            SaveData save = new SaveData();
            //加载XML文档
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(savePathByXML);

            #region 列表的读取方式
            ////通过节点名称来获取元素，结果为XmlNodeList类型
            //XmlNodeList targets = xmlDoc.GetElementsByTagName("target");
            ////遍历所有的target节点，并获得子节点和子节点的InnerText
            //if (targets.Count != 0)
            //{
            //    foreach (XmlNode target in targets)
            //    {
            //        XmlNode targetPosition = target.ChildNodes[0];
            //        int targetPositionIndex = int.Parse(targetPosition.InnerText);
            //        //把得到的值存储到save中
            //        save.livingTargetPositions.Add(targetPositionIndex);

            //        XmlNode monsterType = target.ChildNodes[1];
            //        int monsterTypeIndex = int.Parse(monsterType.InnerText);
            //        save.livingMonsterTypes.Add(monsterTypeIndex);
            //    }
            //}
            #endregion

            //得到存储的射击数和分数
            XmlNodeList shootNum = xmlDoc.GetElementsByTagName("shootNum");
            int shootNumCount = int.Parse(shootNum[0].InnerText);
            save.shootNum = shootNumCount;

            XmlNodeList score = xmlDoc.GetElementsByTagName("score");
            int scoreCount = int.Parse(score[0].InnerText);
            save.score = scoreCount;

            //数据读取
            LocalData.score = save.score;
            LocalData.shootNum = save.shootNum;
            Debug.Log("读取成功！");

        }
        else
        {
            Debug.Log("存档文件不存在");
        }
    }
    #endregion



}

public enum SaveType
{
    Bin,
    XML,
    Json
}

[Serializable]
public class SaveData
{
    public int shootNum = 0;
    public int score = 0;
}

