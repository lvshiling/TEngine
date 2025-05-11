using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
namespace GameLogic
{
    public class TestListCell : MonoBehaviour
    {
        //这里想办法自动生成绑定代码
        public Image image;
        public Text text;
        public Button button;

        public int index;
        public UnityAction<int> clickAction;
        private void Start()
        {
            if (button != null)
                button.onClick.AddListener(ButtonClick);
        }
        void ButtonClick()
        {
            clickAction?.Invoke(index);
        }

    }
}