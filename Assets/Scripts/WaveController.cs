﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveController : MonoBehaviour
{
    public System.Action EventWaveFinished = null;

    [System.Serializable]
    public class MeteorSpawnSettings
    {
        public MeteorType m_type;
        public GameObject m_prefab;
        public ParticleSystem m_trailEffect;
        public ParticleSystem m_hitEffect;
    }

    void Start()
    {
        
    }

    public void StartWave()
    {
        // m_wavesList[m_indexWave].Meteors[m_indexMeteor]
        // получает волну по индексу волны
        // увеличивает индекс волны
        // сбрасывает в 0 индекс метеоритов
        // выставляет  в true флаг m_waveInProcess
        //MeteorType currentIndexM;
        if (m_indexWave >= m_wavesList.Count)
        {
            return;
        }

        m_currentWave = m_wavesList[m_indexWave];
        m_indexWave++;
        m_indexMeteor = 0;
        m_waveInProcess = true;
    }

    void Update()
    {
        if (!m_waveInProcess)
        {
            return;
        }

        m_timer += Time.deltaTime;
        if (m_timer >= m_timeSettings)
        {
            MeteorType currType;
            currType = m_currentWave.MeteorType[m_indexMeteor];

            foreach (var currMeteor in m_settings)
            {
                if (currType == currMeteor.m_type)
                {
                    float posX = Random.Range(-5.0f, 4.0f);
                    GameObject meteor = Instantiate(currMeteor.m_prefab);
                    meteor.transform.position = new Vector2(posX, 6.7f);
                    float rotationZ = Random.Range(0f, 360f);
                    meteor.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
                    GameObject trailEffect = Instantiate(currMeteor.m_trailEffect.gameObject);
                    trailEffect.gameObject.transform.SetParent(meteor.transform);
                    trailEffect.transform.localPosition = Vector2.zero;
                    GameObject hitEffect = Instantiate(currMeteor.m_hitEffect.gameObject);
                    hitEffect.gameObject.transform.SetParent(meteor.transform);
                    hitEffect.transform.localPosition = Vector2.zero;
                    m_timer = 0.0f;
                    m_meteors.Add(meteor.GetComponent<Meteor>());
                }
            }
            m_indexMeteor++;

            if (m_indexMeteor >= m_currentWave.MeteorType.Count)
            {
                m_waveInProcess = false;
                if (EventWaveFinished != null)
                {
                    EventWaveFinished();
                }
            }
        }
    }

    public List<Wave> Waves
    {
        get { return m_wavesList; }
        set { m_wavesList = value; }
    }

	public bool WaveInProcess
	{
		get { return m_waveInProcess; }
	}

    [SerializeField]
    private List<Wave> m_wavesList = null;
    [SerializeField]
    private List<MeteorSpawnSettings> m_settings = new List<MeteorSpawnSettings>();
    [SerializeField]
    private float m_timeSettings = 0.0f;
    private float m_timer = 0.0f;
    private List<Meteor> m_meteors = new List<Meteor>();
    private Wave m_currentWave;
    private int m_indexWave = 0;
    private int m_indexMeteor = 0;
    private bool m_waveInProcess = false;
}
