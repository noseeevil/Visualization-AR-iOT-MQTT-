using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;

using System;
using UnityEngine.UI;
using System.Threading;

public class mqttTest : MonoBehaviour 
{
	private MqttClient client;

    public string BrokerIp= "192.168.42.1";
    public int BrokerPort = 1883;
    public string TopicCO2 = "";
    public string TopicHumidity = "";
    public string TopicIlluminance = "";
    public string TopicSoundLevel = "";
    public string TopicTemperature = "";
    public string TopicInputVoltage = "";
    public string TopicBatVoltage = "";
    public string MessageText = "";


    public Text ConnectStatusText;


    public Text TextCO2;
    public Text TextHumidity;
    public Text TextIlluminance;
    public Text TextSoundLevel;
    public Text TextTemperature;
    public Text TextVoltage;
    public Text TextBatVoltage;

    private void Start()
    {
        //Debug.Log("Screen - " + Screen.width.ToString() + "  " + Screen.height.ToString());
    }

    public bool can = false;
    private void Update()
    {
        if(can)
        {
            can = false;
            Send(TopicCO2, MessageText);
        }
        TextCO2.text = "CO2(ppm) - " + co2;
        //TextHumidity.text = "Влажность(г/м³) - " + humidity;
        TextHumidity.text = "Влажность(%) - " + humidity;
        TextIlluminance.text = "Освещение - " + illuminance;
        TextSoundLevel.text = "Шум(дБ) - " + sound;
        TextTemperature.text = "Температура(°C) - " + temperature;
        TextVoltage.text = "Наряжение(В) - " + voltage;
        TextBatVoltage.text = "Напряжение батареи(В) - " + voltageBat;
    }

    // Use this for initialization
    public void Connect() 
    {

        var t = new Thread(StartThread)
        {
            IsBackground = true
        };
        t.Start();
    }

    public void StartThread()
    {
        client = new MqttClient(IPAddress.Parse(BrokerIp), BrokerPort, false, null);

        client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

        string clientId = Guid.NewGuid().ToString();

        client.Connect(clientId);

        Debug.Log("Connected - " + BrokerIp);
        //ConnectStatusText.text = "Connected - " + BrokerIp;

        client.Subscribe(new string[] { TopicCO2 }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { TopicHumidity }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { TopicIlluminance }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { TopicSoundLevel }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { TopicTemperature }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { TopicInputVoltage }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        client.Subscribe(new string[] { TopicBatVoltage }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
	{
        GetShortString(e.Topic, System.Text.Encoding.UTF8.GetString(e.Message));
        Debug.Log(e.Topic);
        //Debug.Log("Received: " + System.Text.Encoding.UTF8.GetString(e.Message));
	} 

    public void Send(string topic, string msg)
    {
        client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        ConnectStatusText.text = "Sent - " + topic + "/" + msg;
    }

    public void Send()
    {
		Debug.Log("[Sending...]");
        //client.Publish("hello/world", System.Text.Encoding.UTF8.GetBytes("Sending from Unity3D!!!"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        client.Publish(TopicCO2, System.Text.Encoding.UTF8.GetBytes(MessageText), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
        Debug.Log("[Sent]");
	}

    private string co2 = "";
    private string humidity = "";
    private string illuminance = "";
    private string sound = "";
    private string temperature = "";
    private string voltage = "";
    private string voltageBat = "";

    private void GetShortString(string inputTopic, string inputMsg)
    {
        if(inputTopic == "/devices/wb-msw2_34/controls/CO2")
        {
            Debug.Log("CO2 - " + inputMsg);
            //TextCO2.text = "huy";
            co2 = inputMsg;
        }
        if (inputTopic == "/devices/wb-msw2_34/controls/Humidity")
        {
            //TextHumidity.text = "Humidity - " + inputMsg;
            Debug.Log("Humidity - " + inputMsg);
            humidity = inputMsg;
        }
        if (inputTopic == "/devices/wb-msw2_34/controls/Illuminance")
        {
            //TextIlluminance.text = "Illuminance - " + inputMsg;
            Debug.Log("Illuminance - " + inputMsg);
            illuminance = inputMsg;
        }
        if (inputTopic == "/devices/wb-msw2_34/controls/Sound Level")
        {
            //TextSoundLevel.text = "Sound Level - " + inputMsg;
            Debug.Log("Sound Level - " + inputMsg);
            sound = inputMsg;
        }
        if (inputTopic == "/devices/wb-msw2_34/controls/Temperature")
        {
            //TextTemperature.text = "Temperature - " + inputMsg;
            Debug.Log("Temperature - " + inputMsg);
            temperature = inputMsg;
        }
        if (inputTopic == "/devices/wb-msw2_34/controls/Input Voltage")
        {
            //TextTemperature.text = "Temperature - " + inputMsg;
            Debug.Log("InputVoltage - " + inputMsg);
            voltage = inputMsg;
        }
        if (inputTopic == "/devices/wb-adc/controls/BAT")
        {
            //TextTemperature.text = "Temperature - " + inputMsg;
            Debug.Log("InputBatVoltage - " + inputMsg);
            voltageBat = inputMsg;
        }
    }

    public void RemoteControl(string msg)
    {
        Send("/devices/shedule0/controls/virtual/on", msg);
    }

    public void VirtualNight(string msg)
    {
        Send("/devices/shedule7/controls/virtual_night/on", msg);
    }

    public void VirtualLeak(string msg)
    {
        Send("/devices/shedule1/controls/virtual_leak/on", msg);
    }

    public void VirtualRadiation(string msg)
    {
        Send("/devices/shedule5/controls/virtual_radiation/on", msg);
    }

    public void VirtualVoltage(string msg)
    {
        Send("/devices/shedule3/controls/virtual_voltage/on", msg);
    }

    public void VirtualNose1(string msg)
    {
        Send("/devices/shedule4/controls/virtual_nose1/on", msg);
    }

    public void VirtualNose2(string msg)
    {
        Send("/devices/shedule4/controls/virtual_nose2/on", msg);
    }

    public void VirtualIntruder(string msg)
    {
        Send("/devices/shedule2/controls/virtual_intruder/on", msg);
    }

}
