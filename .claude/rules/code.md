# 코드 지침 (Unity C#)

## 간결하게
- 지금 필요한 기능만 구현한다. 미래 대비용 인터페이스, 베이스 클래스, 매니저 계층을 만들지 않는다.
- 추상화는 실제 사용처가 2곳 이상일 때만 한다.
- 스크립트 하나에 책임 하나, 100줄 이내를 목표로 한다.
- 주석은 코드만으로 이유를 알 수 없을 때만 쓴다. XML 문서 주석과 코드를 되풀이하는 주석은 쓰지 않는다.
- 일어날 수 없는 상황을 위한 null 체크, try/catch를 쓰지 않는다. 필수 참조는 `[SerializeField]`로 받는다.
- 중첩 대신 early return을 쓴다.

## 최적화 (WebGL 기준)
- `Update` 등 매 프레임 경로에서 힙 할당을 하지 않는다: LINQ, 문자열 결합, 새 컬렉션, boxing, 캡처 람다.
- 반복 생성되는 오브젝트는 `UnityEngine.Pool.ObjectPool`을 쓴다. 플레이 중 `Instantiate`/`Destroy`를 반복하지 않는다.
- `GetComponent`, `Camera.main`, `Find` 계열은 `Awake`에서 한 번만 호출해 캐시한다.
- 할 일 없는 `Update`는 두지 않는다. 상태 변화는 이벤트로 전달하고, UI는 값이 바뀔 때만 갱신한다.
- 클릭 대상은 UGUI `Button`으로 만든다. `Collider2D`와 `Rigidbody2D`는 쓰지 않는다.
- 매 프레임 움직이는 UI는 별도 `Canvas`에 두어 나머지 UI가 다시 배치되지 않게 한다.
- 측정 없이 가독성을 해치는 미세 최적화는 하지 않는다.

## 데이터
- 할당량, 점수, 속도 같은 수치는 `GameConfig` ScriptableObject에 모은다.
