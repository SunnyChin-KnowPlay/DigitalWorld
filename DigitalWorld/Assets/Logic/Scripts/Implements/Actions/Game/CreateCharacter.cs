/**
 * 该文件通过代码生成器生成
 * 默认模板的回调函数会抛出NotImplementedException异常
 * 在创建对应的行动后，建议第一时间实现函数效果。
 */
using System;

namespace DigitalWorld.Logic.Actions.Game
{
    public partial class CreateCharacter
    {
        /// <summary>
        /// 当进场时的回调
        /// 如果是延时类行动 则该函数实现"进场"效果 例如给目标对象挂上效果之类的
        /// 如果是瞬时类行动 则将实现直接写入该函数
        /// </summary>
        protected override void OnEnter()
        {
            base.OnEnter();

            throw new NotImplementedException();
        }

        /// <summary>
        /// 当退场时的回调
        /// 如果是延时类行动 则该函数实现"退场"效果 例如给目标对象移除效果之类的
        /// 如果是瞬时类行动 则可以忽视该函数 通常情况下 该函数会在进场的同一帧内被调用 建议留空该函数
        /// </summary>
        protected override void OnExit()
        {
            base.OnExit();

            throw new NotImplementedException();
        }

        /// <summary>
        /// 迭代回调
        /// 如果是延时类行动 则该函数实现具体的迭代功能
        /// 如果是瞬时类行动 则建议忽视该函数 留空
        /// </summary>
        /// <param name="delta"></param>
        protected override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);

            throw new NotImplementedException();
        }
    }
}
