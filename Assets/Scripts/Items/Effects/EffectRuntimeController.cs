using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

[DisallowMultipleComponent]
public class EffectRuntimeController : MonoBehaviour
{
    // questo componente si occupa di applicare e gestire gli effetti temporanei degli item, come ad esempio un boost alla velocità o all'altezza del salto
    // è un componente runtime che viene aggiunto dinamicamente al player quando si usa un item
    [System.Serializable]
    private class TimedStatModifier
    {
        public PlayerStatType statType;
        public float multiplier;
        public float endTime;
    }

    private readonly List<TimedStatModifier> _activeModifiers = new List<TimedStatModifier>();

    private ThirdPersonController _controller;
    private float _baseMoveSpeed;
    private float _baseJumpHeight;

    private bool _isInitialized;

    private void Awake()
    {
        _controller = GetComponent<ThirdPersonController>();
        if (_controller == null)
        {
            enabled = false;
            return;
        }

        RecordInitialStats(); // mi salvo le stats base del player
    }

    private void Update()
    {
        if (!_isInitialized)
        {
            return;
        }

        bool removedAny = false; // flag per capire se devo ricalcolare le stats dopo aver rimosso i modificatori scaduti
        float now = Time.time;
        for (int i = _activeModifiers.Count - 1; i >= 0; i--)
        {
            if (_activeModifiers[i].endTime <= now)
            {
                _activeModifiers.RemoveAt(i);
                removedAny = true;
            }
        }

        if (removedAny)
        {
            RecalculateStats();
        }
    }

    public bool ApplyTimedStatMultiplier(PlayerStatType statType, float multiplier, float durationSeconds)
    {
        if (!_isInitialized || durationSeconds <= 0f || Mathf.Approximately(multiplier, 0f))
        {
            return false;
        }

        _activeModifiers.Add(new TimedStatModifier
        {
            statType = statType,
            multiplier = multiplier,
            endTime = Time.time + durationSeconds
        });

        RecalculateStats();
        return true;
    }

    private void RecordInitialStats()
    {
        _baseMoveSpeed = _controller.MoveSpeed;
        _baseJumpHeight = _controller.JumpHeight;
        _isInitialized = true;
    }

    private void RecalculateStats()
    {
        float moveMul = 1f;
        float jumpMul = 1f;

        for (int i = 0; i < _activeModifiers.Count; i++)
        {
            TimedStatModifier modifier = _activeModifiers[i];
            // se in futuro aggiungo altri tipi di stat, qui dovrò aggiungere altri if per gestirli
            // se ho molti tipi di stat, potrebbe essere meglio usare un dizionario PlayerStatType -> multiplier invece di tanti if, ma per ora con 2 stat va bene così
            if (modifier.statType == PlayerStatType.MoveSpeed)
            {
                moveMul *= modifier.multiplier;
                continue;
            }

            if (modifier.statType == PlayerStatType.JumpHeight)
            {
                jumpMul *= modifier.multiplier;
            }
        }

        _controller.MoveSpeed = _baseMoveSpeed * moveMul;
        _controller.JumpHeight = _baseJumpHeight * jumpMul;
    }
}
