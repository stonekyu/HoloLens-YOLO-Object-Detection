# 1. General
![Demo image](Demo.jpg)

이 Unity 프로젝트는 Unity 프레임워크 Sentis를 사용하여 HoloLens 2 하드웨어에서 2D 이미지 객체 인식 모델을 실행하는 데 사용됩니다. 예시로 YOLOv8n과 YOLOv10n 모델이 사용되었습니다(기본 모델이 포함되어 있습니다).
모델은 애플리케이션이 시작되자마자 실행됩니다. 인식된 객체는 클래스와 관련된 인식 확률을 표시하는 레이블로 식별됩니다(이미지에서 볼 수 있듯이). 이 레이블은 공간 메시(spatial mesh)에 구를 발사하여 인식된 객체의 중앙에 배치됩니다.

이미지의 객체 2D 위치와 카메라 및 공간 메시 정보를 결합하여 객체의 3D 공간에서 위치를 추정할 수 있습니다. 이미지는 카메라의 시야에 맞게 정렬된 비율로 카메라 앞에 가상으로 배치됩니다. 아래 이미지와 같이 카메라 A의 위치에서 시작해 이미지 B의 객체 위치를 통과하는 구를 발사하여 공간 메시 C에 충돌하는 지점에서 3D 공간에서 객체의 위치를 추정할 수 있습니다.

![Projection](Projection.png)

### 새 객체 감지 및 사라진 객체 제
인식 오류를 줄이고 안정성을 높이기 위해 추가 필터 메커니즘이 통합되었습니다. 새로운 객체는 모델이 연속 4회 실행에서 인식된 경우에만 사용자의 시야에 표시됩니다. 또한 객체가 모델 실행에서 인식되지 않더라도 즉시 제거되지 않습니다. 사용자의 시야에서 3초 동안 인식되지 않은 경우에만 객체가 제거됩니다. 객체가 사용자의 시야에 있지 않으면 해당 위치에 남아 있습니다.

### Features based on detections
이 예제 코드에서는 인식된 각 객체에 대해 레이블과 선택적인 디버그 정보가 표시됩니다. 인식된 객체를 기반으로 자체 기능을 실행하려면 YoloRecognitionHandler 클래스의 TriggerDetectionActions 메서드를 확장하면 됩니다.

# 2. Settings & Parameters
여러 설정은 hand menu에서 변경할 수 있습니다. 프로그램의 매개변수는 Parameters 파일에서 변경할 수 있습니다. 구현과 관련된 매개변수는 다음과 같습니다.

### Model parameters
- `ModelImageResolution`: 모델 입력 이미지의 해상도를 정의합니다. YOLO 기본 입력 크기는 640 x 640입니다.
- `ModelVersion`: 사용된 YOLO 버전을 구분합니다. 현재 v8 및 v10이 지원됩니다.
- `OverlapThreshold`: 두 경계 상자가 동일한 객체를 설명하는지 여부를 정의합니다(IoU로 측정).

### Performance presets
이 값들은 모델 실행 성능에 대해 hand menu에 저장된 프리셋을 나타냅니다. 값은 매 프레임에서 실행되는 레이어 수를 설명합니다.
- `LayersHigh`: 높은 모델 실행률이지만 낮은 프레임률을 위한 프리셋.
- `LayersLow`: 낮은 모델 실행률이지만 높은 프레임률을 위한 프리셋.

### Model recognition accuracy
이 값들은 손 메뉴에 저장된 모델 인식의 정확도 프리셋을 설명합니다. 값은 개별 인식이 실제로 인식된 것으로 간주되기 위한 최소 정확도를 설명합니다.
- `ThresholdHigh`
- `ThresholdMedium`
- `ThresholdLow`

### Debug options
디버깅 목적으로 손 메뉴에서 활성화할 수 있는 옵션입니다.
- `Bounding boxes`: 가상 투영 평면에 감지된 객체의 경계 상자를 시각화합니다.
- `Model debug image`: 모델에 입력되는 이미지를 표시하고 이미지 내 감지를 위한 경계 상자를 그립니다.
- `Projection cubes`: 이미지의 고정된 위치를 가상 투영 평면의 투영된 위치에 큐브로 시각화합니다. 이를 통해 이미지와 방 안의 위치가 일치하는지 확인할 수 있습니다.
- `Debug sphere cast`: 발사된 구를 시각화합니다.

### Position calculator parameters (2D => 3D)
이 매개변수 그룹은 이미지의 2D 인식을 기반으로 한 3D 위치 추정과 관련이 있습니다. 이 매개변수의 값은 디버그 옵션을 사용하여 결정되었습니다. 
- `VirtualProjectionPlaneWidth`: 투영 평면의 너비를 설명합니다(이미지에서 B로 표시됨).
- `MaxSphereLength`: 객체가 표시되는 최대 거리입니다.
- `SphereCastSize`: 메시로부터 HoloLens로 발사된 구의 크기입니다(이 값이 클수록 작은 객체를 맞추기가 쉬워집니다).
- `HeightOffset`: 투영 평면의 오프셋입니다.
- `SphereCastOffset`: 구 발사의 시작 위치 오프셋입니다. 이는 HoloLens 카메라의 위치를 눈에 상대적으로 나타냅니다.

### Parameters for features based on Object Recognition
- `MinTimesSeen`: 모델이 객체를 감지하여 사용자가 볼 수 있을 때까지 실행된 횟수입니다.
- `ObjectTimeOut`: 객체가 삭제되기까지 해당 객체가 나타나지 않은 시간(초)입니다.
- `MaxIdenticalObject`: 서로 다른 감지에서 동일한 클래스로 인식되는 두 객체 사이의 최대 거리입니다.

# 3. Custom model

사용자 지정 모델을 사용하려면 다음 단계를 수행하십시오.
1. [YOLOv8n](https://docs.ultralytics.com/modes/train/#usage-examples) 또는 [YOLOv10n](https://docs.ultralytics.com/models/yolov10/#usage-examples) 모델을 훈련합니다/
2. 훈련된 모델을 ONNX로 내보냅니다. [Yolov8](https://docs.ultralytics.com/modes/export/#usage-examples) / [Yolov10](https://github.com/THU-MIG/yolov10#Export)을 참조하세
3. 훈련된 모델을 Assets/Models 폴더에 복사합니다.
4. YoloObjectLabeler 프리팹 또는 장면에서 연결된 모델을 업데이트합니다.
5. ObjectClass 스크립트에서 감지된 클래스를 업데이트합니다.
- **참고:** 훈련용 config.yaml에 나열된 순서와 동일해야 합니다.
6. 모델의 입력 해상도를 변경한 경우 Parameters 스크립트에서 ModelImageResolution을 업데이트합니다(기본값: 640 x 640).

성능 권장 사항:
1. 모델이 구별해야 하는 클래스가 적을수록 성능이 향상됩니다. 기본 모델에는 많은 클래스가 포함되어 있어 상대적으로 느립니다.
2. 16비트 양자화는 거의 영향을 미치지 않습니다(8비트는 시도하지 않았음).

YOLO 외에도 다른 ONNX 모델도 사용 가능합니다. 이를 위해 YoloModelOutputProcessor 클래스에서 모델 출력 텐서를 읽는 로직을 수정해야 합니다.

# 4. HoloLens 배포

HoloLens에 최신 버전의 앱을 설치하려면 다음 단계를 수행하십시오.
1. 필수 소프트웨어 설치: 
   - git
   - Universal Windows Platform Build Support 모듈이 포함된 Unity 버전 2021.3.22f1
     - 권장 사항: Unity와 함께 Visual Studio를 개발 도구로 설치하지 마십시오. Visual Studio 2019만 제공됩니다.
   - 다음 워크로드 및 구성 요소가 포함된 Visual Studio 2017 이상(권장: 2022)
     - `.NET desktop development` workload
     - `Desktop development with C++` workload
     - `Universal Windows Platform development` workload
       - 설치 세부 정보에서 다음 구성 요소가 포함되었는지 확인하십시오:
         - `USB-Connectivity` 
         - `C++ (vNNN) Universal Windows Platform tools` (최신 버전)
         - 모 `Windows SDK` (권장: Windows 11)
     - `Game development with Unity` workload
     - `C++ Universal Windows Platform support for vNNN build tools (ARM64)`component
     - `MSVC vNNN - VS NNNN C++ ARM build tools (Latest)` component
     - `MSVC vNNN - VS NNNN C++ ARM64 build tools (Latest)` component
2. 로컬 컴퓨터에서 개발자 모드 활성화
3. HoloLens에서: `설정`->`업데이트 및 보안ㅇ`->`개발자용`으로 이동하여 `개발자 기능 사용` 및 `장치 검색`을 활성화합니다.
4. git 저장소를 클론합니다.
5. Unity에서 프로젝트를 엽니다.
6. 유니티에서 `File`->`Build Settings`을 선택합니다.
   - `HoloLensYOLOObjectDetectionScene`이 Build Settings에서 선택되었는지 확인하세요.
   - `Universal Windows Platform`이 플랫폼으로 선택되었는지 확인하세요.
   - `ARM-64`가 `Build Target Platform`으로 선택되었는지 확인하세요.
   - *선택사항:* `Player Settings...`에서 앱의 이름을 변경하려면`Product name` 밒 `Package name`에 원하는 이름을 입력하세요 (`Universal Windows Platform`->`Publishing settings`에서 설정가)
   - build를 클릭하고 대상 폴더를 선택하세요 (권장: 빈 폴더).
   - **참고:** HoloLens에 동일한 이름의 앱이 이미 설치되어 있는 경우, 종종 새 버전이 덮어쓰이지 않을 수 있습니다. 이 경우 이름을 변경해야 합니다. 배포 중 오류 코드 0x80070057이 발생할 때도 이와 같은 문제가 발생할 수 있습니다.
7. Visual Studio에서 빌드된 솔루션을 엽니다
8. Visual Studio에서 빌드 대상으로 `Release`, `ARM64`, `Device`를 선택합니다 (상단 툴바에서 설정).
9. HoloLens를 PC와 USB 케이블로 연결합니다.
   - **참고:** Windows 탐색기에 HoloLens가 나타나야 합니다.
10. Visual Studio에서 `Build`를 클릭합니다.
   - **참고:** PC에서 HoloLens로 처음 배포할 때, 배포 과정 중에 PIN 입력 요청이 나타날 수 있습니다. 이 PIN은 HoloLens에서 다음과 같이 확인할 수 있습니다: `설정`->`업데이트 및 보안`->`개발자용`->`장치 검색` 아래에서 `Pair`를 클릭힙니다.

# 5. Windows device portal

Windows Device Portal은 HoloLens의 영상을 PC로 스트리밍하거나 HoloLens에서 실행 중인 앱의 성능을 분석하는 데 사용할 수 있습니다. 이 포털을 열려면 다음 단계를 따르세요:
1. HoloLens를 PC와 USB 케이블로 연결하거나 PC와 HoloLens를 동일한 WLAN 네트워크에 연결합니다.
2. HoloLens에서: `설정`->`업데이트 및 보안`->`개발자용`으로 이동하여 `Device Portal`을 활성화합니다.
3. HoloLens에서: `설정`->`업데이트 및 보안`->`개발자용`으로 이동하여 `Device Portal`아래에 표시된 첫 번째 URL을 PC의 브라우저에 입력합니다.
4. 비디오 스트림은 `Views`->`Mixed Reality Capture`->`Live preview`에서 사용할 수 있습니다.

# 6. Debugging

## PC에서
Unity에서 모델 실행을 테스트하려면 [OBS](https://obsproject.com/)의 가상 카메라를 사용하여 모델의 카메라 입력으로 사용할 수 있습니다.
이 경우 객체는 이미지 내 위치에 따라 사용자에게 고정된 거리에서 투영됩니다.
## HoloLens에서
HoloLens에서 C# 코드를 직접 디버깅하려면 다음 단계를 따르세요(strongly based on https://stackoverflow.com/a/59990792):
1. `Project Settings`->`Player`에서 `PrivateNetworkClientServer` 및 `InternetClientServer` 기능을 활성화합니다.
2. `Build Settings`에서 `Development Build`, `ScriptDebugging`, `Wait For Managed Debugger`를 활성화합니다.
   - **참고:** `Build configuration`을 `Debug` 로 변경하면 디버깅 성공 확률이 높아집니다. 그러나 성능이 더 저하됩니다. 이 옵션이 선택된 경우 Visual Studio에서 배포할 때 `Realease` 대신 `Debug`를 선택해야 합니다
3. 프로젝트를 빌드하고 배포합니다.
4. PC와 HoloLens를 동일한 WLAN에 연결합니다 (WLAN은 멀티캐스트를 지원해야 함 -> 권장: PC에서 핫스팟 생성).
5. USB 케이블을 HoloLens에서 분리하고 앱을 시작합니다 -> 팝업이 나타납니다.
6. Visual Studio에서 `HoloLens-YOLO-Object-Detection.sln`을 열고 `Debug`->`Attach Unity Debugger`를 클릭합니다. 목록에 HoloLens가 나타납니다 -> HoloLens를 선택하세요.
    - `HoloLens-YOLO-Object-Detection.sln` 파일이 저장소 폴더에 없는 경우:
        - Unity에서 `Edit`->`Preferences`->`External tools`를 선택하세요.
        - `External Script Editor`로 Visual Studio를 선택하세요.
        - `Embedded packages` 및 `Local packages`를 선택하세요
        - `Regenerate project files`를 클릭하세요.
7. HoloLens에서 팝업을 닫습니다.
8. Visual Studio에서 break point를 설정하고 앱을 디버깅합니다.

**참고:** 디버깅 중에는 앱 속도가 상당히 느려지며 배포에도 시간이 더 소요됩니다.

# 7. Acknowledgment
이 프로젝트는 [HoloLens-YOLO-Object-Detection]([https://github.com/LocalJoost/YoloHolo/tree/main](https://github.com/dangberg/HoloLens-YOLO-Object-Detection))에서 가져왔으며, HoloLens에서 내가 학습한 YOLO 모델을 사용하여 객체 감지를 구현하기 위해 일부 수정되었습니다. 원본 프로젝트는 유사한 기능을 제공하였고, 이를 바탕으로 나만의 YOLO 모델을 통합하여 프로젝트를 완성할 수 있었습니다. README 파일은 원래 영어로 작성된 내용을 한국어로 번역하고 나의 프로젝트 요구 사항에 맞게 보완하였습니다.
