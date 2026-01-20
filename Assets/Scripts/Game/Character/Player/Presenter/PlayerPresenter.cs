using Core;
using TMPro;
using UniRx;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private StatsContext _statsContext;

    private void Awake()
    {
        _statsContext.Get<Health>().Current.Subscribe( s => _healthText.text = s.ToString());
    }
}