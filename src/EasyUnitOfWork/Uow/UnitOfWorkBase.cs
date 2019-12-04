using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyUnitOfWork.Uow
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        /// <summary>
        /// 工作单元id
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// 对外公布的工作单元对象
        /// </summary>
        public IUnitOfWork Outer { get; set; }

        /// <summary>
        /// 提交事件
        /// </summary>
        public event EventHandler Completed;

        /// <summary>
        /// 异常事件
        /// </summary>
        public event EventHandler<Exception> Failed;

        /// <summary>
        /// 事件
        /// </summary>
        public event EventHandler Disposed;


        /// <summary>
        /// 工作单元创建选项
        /// </summary>
        public UnitOfWorkOptions Options { get; protected set; }

        /// <summary>
        /// 工作单元的附加选项
        /// </summary>
        public Dictionary<string, object> Items { get; set; }

        /// <summary>
        /// 是否已经释放了资源
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// 是否已经启动了工作单元
        /// </summary>
        private bool _isBeginCalledBefore;

        /// <summary>
        /// 是否已经提交了工作单元
        /// </summary>
        private bool _isCompleteCalledBefore;

        /// <summary>
        /// 工作单元是否已经提交成功
        /// </summary>
        private bool _succeed;

        /// <summary>
        /// 工作单元异常
        /// </summary>
        private Exception _exception;

        /// <summary>
        /// 构造函数
        /// </summary>
        protected UnitOfWorkBase()
        {
            Id = Guid.NewGuid().ToString("N");
            Items = new Dictionary<string, object>();
        }

        /// <inheritdoc/>
        public void Begin(UnitOfWorkOptions options)
        {

            PreventMultipleBegin();
            Options = options; //TODO: Do not set options like that, instead make a copy?



            BeginUow();
        }

        /// <summary>
        /// 保存修改
        /// </summary>
        public abstract void SaveChanges();

        /// <summary>
        /// 保存修改 - 异步
        /// </summary>
        /// <returns></returns>
        public abstract Task SaveChangesAsync();


        /// <summary>
        /// 提交工作单元
        /// </summary>
        public void Complete()
        {
            PreventMultipleComplete();
            try
            {
                CompleteUow();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <summary>
        /// 提交工作单元 - 异步
        /// </summary>
        /// <returns></returns>
        public async Task CompleteAsync()
        {
            PreventMultipleComplete();
            try
            {
                await CompleteUowAsync();
                _succeed = true;
                OnCompleted();
            }
            catch (Exception ex)
            {
                _exception = ex;
                throw;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_isBeginCalledBefore || IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (!_succeed)
            {
                OnFailed(_exception);
            }

            DisposeUow();
            OnDisposed();
        }

        /// <summary>
        /// 启动工作单元
        /// </summary>
        protected virtual void BeginUow()
        {

        }

        /// <summary>
        /// 提交工作单元
        /// </summary>
        protected abstract void CompleteUow();

        /// <summary>
        /// 提交工作单元 - 异步
        /// </summary>
        protected abstract Task CompleteUowAsync();

        /// <summary>
        /// 释放工作单元
        /// </summary>
        protected abstract void DisposeUow();



        /// <summary>
        /// Called to trigger <see cref="Completed"/> event.
        /// </summary>
        protected virtual void OnCompleted()
        {
            Completed?.Invoke(this, null);
        }

        /// <summary>
        /// Called to trigger <see cref="Failed"/> event.
        /// </summary>
        /// <param name="exception">Exception that cause failure</param>
        protected virtual void OnFailed(Exception exception)
        {
            Failed?.Invoke(this, exception);
        }

        /// <summary>
        /// Called to trigger <see cref="Disposed"/> event.
        /// </summary>
        protected virtual void OnDisposed()
        {
            Disposed?.Invoke(this, null);
        }

        private void PreventMultipleBegin()
        {
            if (_isBeginCalledBefore)
            {
                throw new Exception("This unit of work has started before. Can not call Start method more than once.");
            }

            _isBeginCalledBefore = true;
        }

        private void PreventMultipleComplete()
        {
            if (_isCompleteCalledBefore)
            {
                throw new Exception("Complete is called before!");
            }

            _isCompleteCalledBefore = true;
        }

        public override string ToString()
        {
            return $"[UnitOfWork {Id}]";
        }
    }
}
