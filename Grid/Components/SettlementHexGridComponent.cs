// using System;
// using UnityEngine;

// /// <summary>
// /// 管理六边形地块的占领状态
// /// </summary>
// /// <remarks>
// /// 此组件负责:
// /// 1. 存储当前占领该地块的聚落引用
// /// 2. 提供API来更改占领状态
// /// 3. 发送占领变化事件给以下监听者:
// ///    - TerritoryManager (处理领土计算)
// ///    - HexCellVisualController (更新视觉效果)
// ///    - UIManager (更新UI显示)
// /// </remarks>
// [RequireComponent(typeof(HexCell))]
// public class SettlementHexGridComponent : MonoBehaviour
// {
//     // 当前占领的聚落
//     private Settlement _occupyingSettlement;

//     // 事件：当占领状态变化时触发
//     public event EventHandler<OccupationChangedEventArgs> OnOccupationChanged;

//     // 占领状态变化事件参数
//     // 改为普通类
//     public class OccupationChangedEventArgs : EventArgs
//     {
//         public HexCell Cell { get; }
//         public Settlement NewSettlement { get; }
//         public Settlement OldSettlement { get; }

//         public OccupationChangedEventArgs(
//             HexCell cell,
//             Settlement newSettlement,
//             Settlement oldSettlement
//         )
//         {
//             Cell = cell;
//             NewSettlement = newSettlement;
//             OldSettlement = oldSettlement;
//         }
//     }

//     private HexCell _hexCell;

//     private void Awake()
//     {
//         _hexCell = GetComponent<HexCell>();
//     }


//     public Settlement GetOccupyingSettlement() => _occupyingSettlement;

//     // 设置占领聚落并触发事件
//     public void SetOccupyingSettlement(Settlement settlement)
//     {
//         if (_occupyingSettlement == settlement)
//             return;

//         var oldSettlement = _occupyingSettlement;
//         _occupyingSettlement = settlement;

//         // UpdateAppearance();

//         // // 触发事件
//         // // 触发事件 - 以下组件会监听此事件:
//         // // 1. HexCell (更新视觉表现)
//         // // 2. TerritoryManager (更新领土计算)
//         // // 3. UIManager (更新UI显示)
//         // OnOccupationChanged?.Invoke(
//         //     this,
//         //     new OccupationChangedEventArgs(_hexCell, settlement, oldSettlement)
//         // );
//     }

//     // 检查是否可以被占领
//     public bool CanBeOccupied(Settlement settlement)
//     {
//         // todo-实现占领条件检查
//         return true;
//     }
// }
