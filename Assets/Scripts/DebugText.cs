using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    [SerializeField]
    private RectTransform arrow;

    public TextMeshProUGUI F, G, H, P;

    public RectTransform Arrow { get => arrow; set => arrow = value; }
    public RectTransform MyArrow { get => arrow; set => arrow = value; }
    public TextMeshProUGUI F1 { get => F; set => F = value; }
    public TextMeshProUGUI G1 { get => G; set => G = value; }
    public TextMeshProUGUI H1 { get => H; set => H = value; }
    public TextMeshProUGUI P1 { get => P; set => P = value; }
}
