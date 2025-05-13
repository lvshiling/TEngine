using UnityEngine;
using UnityEngine.UI;
using TEngine;
using System.Collections.Generic;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class TestListUI : UIWindow
    {
        #region 脚本工具生成的代码 
        private LoopListHelper _listTest;
        protected override void ScriptGenerator()
        {
            _listTest = FindChildComponent<LoopListHelper>("m_listTest");
        }
        #endregion

        #region 事件 
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

            //测试用数据
            dataList = new List<TestListData>();
            for (int i = 0; i < 50; i++)
            {
                dataList.Add(new TestListData("Name" + i, i));
            }

            //设置刷新cell的委托
            _listTest.itemRenderer = ListCellRenderer;
            //设置列表内元素数量
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
