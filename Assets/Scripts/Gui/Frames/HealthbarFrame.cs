using Assets.Scripts.Gui;
using System;
using UnityEngine;

public class HealthbarFrame : GuiFrame
{
    private HealthbarViewFrame view = null;

    public override string PrefabName
    {
        get { return "HealthbarFrame"; }
    }

    protected override void InitFormControls()
    {
        view = GetView<HealthbarViewFrame>();

        view.HealthText.enabled = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="healthNew">Текущее здоровье</param>
    /// <param name="healthMax">Полное здоровье</param>
    /// <param name="healthCount">Количество полученного урона или лечения</param>
    public void SetHealth(float healthNew, float healthMax, float healthCount)
    {
        if (healthNew <= 0)
        {
            Disable();
        }
        else
        {
            Enable();
            view.HealthbarImage.fillAmount = healthNew / healthMax;

            if (healthCount != 0)
                PlayHealthTextAnimation(healthCount);
        }
    }

    private void PlayHealthTextAnimation(float healthCount)
    {
        if (healthCount < 0)
        {
            view.HealthText.color = Color.red;
        }
        else
        {
            view.HealthText.color = Color.green;
        }

        view.HealthTextMoveEffect.SetPosition(Vector3.zero);
        view.HealthText.enabled = true;
        view.HealthText.text = healthCount.ToString();
        view.HealthTextMoveEffect.MoveConstantTime(new Vector3(0, 100, 0), 1f);
        view.HealthTextMoveEffect.OnFinish += OnHealthTextMoveFinish;
    }

    private void OnHealthTextMoveFinish(object sender, EventArgs e)
    {
        view.HealthText.enabled = false;
    }

    public void SetPosition(Vector3 healthbarHodlerWorldPos)
    {
        Vector3 point = Camera.main.WorldToScreenPoint(healthbarHodlerWorldPos);
        Root.transform.localPosition = point;
    }
}
