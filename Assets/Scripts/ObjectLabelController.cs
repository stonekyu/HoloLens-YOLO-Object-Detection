using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    ///     ��ü ���� �ð��� ��Ҹ� �����ϴ� Ŭ�����Դϴ�.
    /// </summary>
    public class ObjectLabelController : MonoBehaviour
    {
        /// <summary>
        ///     ǥ�õ� ���� �θ� ��ü�Դϴ�.
        /// </summary>
        public GameObject ContentParent;

        /// <summary>
        ///     ��ü�� �߽ɰ� �� ���̿� ���� �׷��ִ� LineRenderer�Դϴ�.
        /// </summary>
        public LineRenderer LineRenderer;

        /// <summary>
        ///     ��ü�� Ŭ������ ǥ���ϴ� �ؽ�Ʈ �޽��Դϴ�.
        /// </summary>
        public TextMeshPro TextMesh;

        /// <summary>
        ///     Ű���� ��ü�� ���� �������Դϴ�.
        ///     TV ��ü�� ���� �������Դϴ�.
        /// </summary>
        public GameObject keyboardPrefab;
        public GameObject tvPrefab;
        
        /// <summary>
        ///     �󺧿� ǥ���� �ؽ�Ʈ�� �����մϴ�.
        /// </summary>
        public string Text
        {
            set => this.TextMesh.text = value;
        }

        /// <summary>
        ///     ��ü ���� ��ġ�� ������Ʈ�մϴ�.
        /// </summary>
        /// <param name="newPosition">��ü�� ���ο� ��ġ�Դϴ�.</param>
        public void UpdatePosition(Vector3 newPosition)
        {
            this.transform.position = newPosition;

            // �ؽ�Ʈ�� ��ü�� �߽� ������ ���� ������Ʈ
            this.LineRenderer.SetPosition(0, this.ContentParent.transform.position);
            this.LineRenderer.SetPosition(1, this.transform.position);
        }

        /// <summary>
        ///     ��� ��ư Ŭ�� �̺�Ʈ�� ó���մϴ�.
        ///     ���� ��Ȱ��ȭ�ϴ� ���, �ش� �������� �����ϴ� ������ �����մϴ�.
        /// </summary>
        public void OnClicked()
        {
            // �� �ؽ�Ʈ�� Ȯ���ϰ�, �ش� �������� �����մϴ�.
            Vector3 spawnPosition = this.transform.position + new Vector3(0, 0.1f, 0); // Y������ 0.1���� ���� ����

            if (this.TextMesh.text == "Keyboard")
            {
                Instantiate(keyboardPrefab, spawnPosition, Quaternion.identity);
            }
            else if (this.TextMesh.text == "Tv")
            {
                Instantiate(tvPrefab, spawnPosition, Quaternion.identity);
            }

            // �������� ������ ��, ObjectLabel�� ��Ȱ��ȭ
            this.gameObject.SetActive(false);
        }
    }
}
