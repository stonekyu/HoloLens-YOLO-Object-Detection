using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    ///     객체 라벨의 시각적 요소를 관리하는 클래스입니다.
    /// </summary>
    public class ObjectLabelController : MonoBehaviour
    {
        /// <summary>
        ///     표시된 라벨의 부모 객체입니다.
        /// </summary>
        public GameObject ContentParent;

        /// <summary>
        ///     객체의 중심과 라벨 사이에 선을 그려주는 LineRenderer입니다.
        /// </summary>
        public LineRenderer LineRenderer;

        /// <summary>
        ///     객체의 클래스를 표시하는 텍스트 메쉬입니다.
        /// </summary>
        public TextMeshPro TextMesh;

        /// <summary>
        ///     키보드 객체를 위한 프리팹입니다.
        ///     TV 객체를 위한 프리팹입니다.
        /// </summary>
        public GameObject keyboardPrefab;
        public GameObject tvPrefab;
        public GameObject chairPrefab;
        public GameObject mousePrefab;

        /// <summary>
        ///     라벨에 표시할 텍스트를 설정합니다.
        /// </summary>
        public string Text
        {
            set => this.TextMesh.text = value;
        }

        /// <summary>
        ///     객체 라벨의 위치를 업데이트합니다.
        /// </summary>
        /// <param name="newPosition">객체의 새로운 위치입니다.</param>
        public void UpdatePosition(Vector3 newPosition)
        {
            this.transform.position = newPosition;

            // 텍스트와 객체의 중심 사이의 선을 업데이트
            this.LineRenderer.SetPosition(0, this.ContentParent.transform.position);
            this.LineRenderer.SetPosition(1, this.transform.position);
        }

        /// <summary>
        ///     토글 버튼 클릭 이벤트를 처리합니다.
        ///     라벨을 비활성화하는 대신, 해당 프리팹을 생성하는 로직만 포함합니다.
        /// </summary>
        public void OnClicked()
        {
            // 라벨 텍스트에서 객체 이름만 추출 (예: "Keyboard 95%" -> "Keyboard")
            string[] labelParts = this.TextMesh.text.Split(' '); // 공백을 기준으로 나눔
            string objectName = labelParts[0]; // 첫 번째 단어 (객체 이름)

            // 객체 이름에 따라 프리팹을 생성
            Vector3 spawnPosition = this.transform.position + new Vector3(0, 0.1f, 0); // Y축으로 1 단위 위에 생성

            if (objectName == "Keyboard")
            {
                Instantiate(keyboardPrefab, spawnPosition, Quaternion.identity);
            }
            else if (objectName == "Tv")
            {
                Instantiate(tvPrefab, spawnPosition, Quaternion.Euler(0, 180, 0));
            }
            else if (objectName == "Chair")
            {
                Instantiate(chairPrefab, spawnPosition, Quaternion.identity);
            }
            else if (objectName == "Mouse")
            {
                Instantiate(mousePrefab, spawnPosition, Quaternion.identity);
            }

            // 프리팹을 생성한 후, ObjectLabel을 비활성화
            this.gameObject.SetActive(false);
        }
    }
}
