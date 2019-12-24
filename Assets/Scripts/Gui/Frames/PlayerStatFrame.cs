using Assets.Scripts;
using Assets.Scripts.Gui;

public class PlayerStatFrame : GuiFrame
{
    private PlayerStatViewFrame view = null;

    public override string PrefabName
    {
        get { return "PlayerStatFrame"; }
    }

    protected override void InitFormControls()
    {
        view = GetView<PlayerStatViewFrame>();
    }

    public void SetData(Stat stat)
    {
        SpriteManager.Instance.SetSprite(view.IconImage, stat.icon);
        view.NameText.text = stat.value.ToString();
    }

    public void SetData(Buff buff)
    {
        SpriteManager.Instance.SetSprite(view.IconImage, buff.icon);
        view.NameText.text = buff.title;
    }
}
