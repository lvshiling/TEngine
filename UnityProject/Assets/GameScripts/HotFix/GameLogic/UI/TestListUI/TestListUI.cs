using UnityEngine;
using UnityEngine.UI;
using TEngine;
using System.Collections.Generic;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class TestListUI : UIWindow
    {
        #region �ű��������ɵĴ���
        private LoopListHelper _listTest;
        protected override void ScriptGenerator()
        {
            _listTest = FindChildComponent<LoopListHelper>("m_listTest");
        }
        #endregion

        #region �¼�
        #endregion


        public class TestListData
        {
            public string userName;
            public int userRank;

            public TestListData(string userName, int userRank)
            {
                this.userName = userName;
                this.userRank = userRank;
            }
        }
        List<TestListData> dataList;
        protected override void OnRefresh()
        {
            base.OnRefresh();

            //����������
            dataList = new List<TestListData>();
            for (int i = 0; i < 50; i++)
            {
                dataList.Add(new TestListData("Name" + i, i));
            }

            //����ˢ��cell��ί��
            _listTest.itemRenderer = ListCellRenderer;
            //�����б���Ԫ������
            _listTest.totalCount = dataList.Count;
        }
        private void ListCellRenderer(Transform trans, int index)
        {
            var data = dataList[index];
            //Debug.Log($"Rendering item at index: {index}, UserName: {data.userName}, UserRank: {data.userRank}");
            var mItem = trans.GetComponent<TestListCell>();
            if (mItem != null)
            {
                mItem.index = index;
                mItem.text.text = $"{data.userName} {data.userRank}";
                mItem.clickAction = OnCellClick;
            }
        }
        void OnCellClick(int index)
        {
            var data = dataList[index];
            Debug.Log($"cell button click {index} , user name {data.userName}" );
        }
    }
}
