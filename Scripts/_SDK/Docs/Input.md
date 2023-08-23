# Input

Input sử dụng UniRx để implement Pub-Sub Pattern

https://sequencediagram.org/
```
title GameInput

User -> GameInput.MoveStream: Publish Move Vector3
MoveSkillBehavior -> GameInput.MoveStream: Subscribe
MoveSkillBehavior -> GameObject: Update Position

```

Source Code:

```
public class GameInput : IInput
{
        public IObservable<Vector3> MoveStream => Observable.EveryUpdate()
                .Where(_ => AbstractGameManager.Instance.StateMachine.State == GameState.Playing
                        && (_joystick.Vertical != 0 || _joystick.Horizontal != 0)
                )
                .Select(_ => Vector3.forward * _joystick.Vertical + Vector3.right * _joystick.Horizontal);
                
}

public class MoveSkillBehavior : MonoBehavior, IHasInput
{
                
        public void SetInput(IInput input)
        {
            input?.MoveStream.Subscribe((moveVector) =>
            {
                OnMove(moveVector);
            }).AddTo(_disposables);
        }
}
```

GameInput cũng có thể Handle UI Input

```
public class GameInput : IInput
{
        public Action<bool> FireAction;
        
        public IObservable<bool> FireStream => Observable.FromEvent<bool>(
        handler => FireAction += handler,
        handler => FireAction -= handler);
 }
 
 GameManager.Instance.GameInput.FightAction?.Invoke(true);
 
 public class FireSkillBehavior : MonoBehavior, IHasInput
{
                
        public void SetInput(IInput input)
        {
            input?.FireStream.Subscribe(_ =>
            {
                OnFire();
            }).AddTo(_disposables);
        }
}
        
```
