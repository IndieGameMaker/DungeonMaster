# VSCode ↔ Unity 연결 문제 해결 기록

- 날짜: 2026-09-11
- 환경: macOS (Apple Silicon), Unity 6000.5.8f1, VSCode + C# Dev Kit 3.20 / C# 2.140 / Unity(vstuc) 1.3.1

---

## 1. 증상

- Unity에서 스크립트를 열어도 VSCode 하단 상태바에 **Project(솔루션) 표시가 안 뜸**
- `MonoBehaviour`, `Transform` 등 **Unity API가 전혀 인식되지 않음** (인텔리센스 없음, 빨간 줄)
- 확장은 전부 설치되어 있고, 프로젝트 파일(`.slnx`, `.csproj`)도 정상 생성됨
- 터미널에서 `dotnet build DungeonMaster.slnx` 는 **성공**함 → 프로젝트 자체는 문제 없음

## 2. 원인

VSCode 로그(`C# Dev Kit.log`)에 아래 에러가 남아 있었다.

```
.NET server STDERR: Failed to load
  /opt/homebrew/Cellar/dotnet/10.0.301/libexec/host/fxr/10.0.9/libhostfxr.dylib
error: dlopen(...) code signature not valid for use in process:
  mapping process and mapped file (non-platform) have different Team IDs
```

### 쉽게 설명하면

| 구성 요소 | 서명 | 비고 |
|---|---|---|
| C# Dev Kit 프로젝트 서버 (VSCode 확장 안의 실행파일) | Microsoft 서명 + **라이브러리 검증 ON** | "나와 같은 팀이 서명한 라이브러리만 로드하겠다" |
| `brew install dotnet` (Homebrew **formula**)의 `libhostfxr.dylib` | **adhoc 서명** (TeamIdentifier 없음) | Homebrew가 소스에서 직접 빌드한 것 |

macOS는 라이브러리 검증이 켜진 프로세스가 **다른 Team ID로 서명된 dylib를 로드하는 것을 차단**한다.
그래서 C# Dev Kit 서버가 Homebrew dotnet을 로드하려다 즉시 죽고 → 솔루션이 열리지 않고 → Roslyn 언어 서버는 임시 `Canonical.csproj`만 잡아서 Unity DLL 참조를 전혀 모르는 상태가 됐다.

### 왜 Homebrew formula를 쓰고 있었나

- `~/.zshrc` 에 `DOTNET_ROOT=/opt/homebrew/opt/dotnet/libexec` 가 PATH 맨 앞에 지정되어 있었음
- 공식 pkg 버전(cask `dotnet-sdk`)은 설치 기록만 남고 실제 파일(`/usr/local/share/dotnet`)이 사라진 상태였음
- 터미널 `dotnet build`는 dotnet 자체 프로세스라 서명 검증에 걸리지 않아 성공했고, 그래서 원인이 바로 드러나지 않았음

## 3. 해결

```bash
# 1) Homebrew formula 제거 (adhoc 서명 버전)
brew uninstall dotnet

# 2) Microsoft 공식 pkg 설치 → /usr/local/share/dotnet
brew reinstall --cask dotnet-sdk
```

`~/.zshrc` 수정:

```bash
# DOTNET 경로
export DOTNET_ROOT=/usr/local/share/dotnet     # 기존: /opt/homebrew/opt/dotnet/libexec
export PATH="$DOTNET_ROOT:$PATH"
```

VSCode **완전 종료(⌘Q) 후 재실행** → 솔루션 로드 정상, Unity API 인식 정상.

### 검증

```bash
which dotnet                 # /usr/local/share/dotnet/dotnet
dotnet --version             # 10.0.401
codesign -dv /usr/local/share/dotnet/host/fxr/*/libhostfxr.dylib 2>&1 | grep TeamIdentifier
# TeamIdentifier=UBF8T346G9  ← Microsoft (adhoc이면 문제 재발)
```

## 4. 앞으로 주의할 점

1. **.NET SDK는 반드시 Microsoft 공식 pkg로 설치한다.**
   - ✅ `brew install --cask dotnet-sdk` 또는 dotnet.microsoft.com 의 pkg
   - ❌ `brew install dotnet` (formula) — VSCode C# Dev Kit과 서명 충돌
2. `DOTNET_ROOT` 는 항상 `/usr/local/share/dotnet` 을 가리키게 유지한다. Homebrew 경로를 넣지 말 것.
3. `brew upgrade` 를 돌릴 때 `dotnet` formula가 의존성으로 다시 딸려오지 않았는지 확인한다.
   ```bash
   brew list --formula | grep dotnet   # 아무것도 안 나와야 정상
   ```
4. **증상이 재발하면 가장 먼저 볼 로그**:
   `~/Library/Application Support/Code/logs/<최신>/window1/exthost/ms-dotnettools.csdevkit/C# Dev Kit.log`
   - `Using existing .NET runtime at ...` 줄이 `/usr/local/share/dotnet` 인지
   - `.NET server STDERR` 줄에 `Team IDs` 에러가 없는지
5. VSCode 설정을 바꾼 뒤에는 `Reload Window` 가 아니라 **완전 종료 후 재실행**해야 `.NET Install Tool` 캐시가 갱신된다.
6. 문제가 안 풀릴 때는 `.vscode/settings.json` 에 경로를 명시할 수 있다:
   ```json
   "dotnetAcquisitionExtension.existingDotnetPath": [
     { "extensionId": "ms-dotnettools.csdevkit", "path": "/usr/local/share/dotnet/dotnet" },
     { "extensionId": "ms-dotnettools.csharp",   "path": "/usr/local/share/dotnet/dotnet" }
   ]
   ```
7. **Rider에는 영향 없음.** Rider는 자체 번들 런타임(`Rider.app/Contents/lib/ReSharperHost/...`)을 사용하고, MSBuild용 CLI 경로는 자동 탐지라 공식 경로를 알아서 잡는다.

## 5. 참고: 프로젝트 파일이 없을 때 (다른 원인)

이번 건은 아니었지만, 같은 증상의 다른 흔한 원인:
- Unity `Edit > Preferences > External Tools` 에서 External Script Editor가 VSCode가 아님
- `Regenerate project files` 를 안 눌러 `.slnx`/`.csproj` 가 없음
- `.vscode/settings.json` 의 `dotnet.defaultSolution` 이 실제 솔루션 파일명과 다름
