<div align = center>


# Virtual Reality - TP1

![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white)
![Microsoft](https://img.shields.io/badge/Microsoft-0078D4?style=for-the-badge&logo=microsoft&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)


## Architecture

```mermaid
classDiagram
direction LR
class KinectStreamsFactory {
    +ctor(kinect: KinectManager)
    -streamFactory : Dictionary~KinectStreams, Func~KinectStream~~ 
    +this[stream: KinectStreams] : KinectStream
}

class KinectStreams {
    <<enum>>
    None
    Color
    Depth
    IR
}

class KinectStream {
    
}

KinectStreamsFactory --> "1" KinectManager
KinectStreamsFactory ..> KinectStreams
KinectStreamsFactory ..> KinectStream
KinectStream --> KinectManager
KinectStream <|-- ColorImageStream
KinectStream <|-- DepthImageStream
KinectStream <|-- InfraredImageStream
KinectStream <|-- BodyStream
```

## Fonctionnalities ✅

- WPF Window
- MVVM Architecture and set-up
- ColorImageStream has been done
- BodyStream has been done
- InfraredImageStream has been done ?
- DepthImageStream has been done ?

## Difficulties and not done ❌

- The Command Button didn't work 
- No UT, but fonctionnal tests
- No "Display Body and Color Streams in the meantime"

</div>