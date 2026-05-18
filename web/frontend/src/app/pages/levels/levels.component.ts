import { Component, OnDestroy, OnInit } from '@angular/core';
import { Navbar } from '../../shared/components/navbar/navbar.component';
import { Footer } from '../../shared/components/footer/footer.component';
import { UnityService } from '../../core/services/unity.service';
import { Level } from '../../core/services/level.service';
import { Router } from '@angular/router';
import { Auth } from '../../core/services/auth.service';
import { Score } from '../../core/services/score.service';

@Component({
  selector: 'app-levels',
  imports: [Navbar, Footer],
  templateUrl: './levels.component.html',
  styleUrl: './levels.component.scss',
})
export class Levels implements OnInit, OnDestroy {

  levels: { id: number, name: string }[] = [];
  nivellsCompletats: number[] = [];

  private boundUnityHandler = this.handleUnityMessage.bind(this);

  constructor(
    private unityService: UnityService,
    private levelsService: Level,
    private authService: Auth,
    private score: Score,
  ) {}

  ngOnInit() {

    this.score.getScoreUser().subscribe({
      next: (completats) => {
        this.nivellsCompletats = completats;
      },
      error: (err) => console.log(err)
    });

  this.levelsService.getLevels().subscribe(data => {
    this.levels = data;
  });

  // Escucha el mensaje que manda Unity al acabar partida
    window.addEventListener('message', this.boundUnityHandler);

     const instanceAnterior = this.unityService.getInstance();
  if (instanceAnterior) {
    instanceAnterior.SendMessage('GameManager', 'DetenerAudio', 'true');
    setTimeout(() => {
      instanceAnterior.Quit().then(() => {
        this.unityService.setInstance(null);
        this.iniciarUnity();
      });
    }, 200);
  } else {
    this.iniciarUnity();
  }
  }

  // Siempre reiniciamos porque el canvas es nuevo cada vez
  //.unityService.setInstance(null);
private iniciarUnity() {
  const token = this.authService.getToken();
  const username = this.authService.getUser().name;

  // @ts-ignore
  createUnityInstance(document.querySelector("#unity-canvas"), {
    dataUrl: "/unity/Build/unity.data",
    frameworkUrl: "/unity/Build/unity.framework.js",
    codeUrl: "/unity/Build/unity.wasm",
    streamingAssetsUrl: "StreamingAssets",
    companyName: "QQClan",
    productName: "NOM-Protocol",
    productVersion: "1.0"
  }).then((instance: any) => {
    this.unityService.setInstance(instance);
    const payload = JSON.stringify({ token, username });

    (window as any).onUnityReady = () => {
      this.unityService.sendMessage('GameManager', 'SetAuthToken', payload);
    };
    setTimeout(() => {
      this.unityService.sendMessage('GameManager', 'SetAuthToken', payload);
    }, 3000);
  });
}

cargarNivel(levelName: string, event: MouseEvent) {
  (event.target as HTMLElement).blur();
  this.unityService.sendMessage('GameManager', 'LoadLevel', levelName);
}

 ngOnDestroy(): void {
    window.removeEventListener('message', this.boundUnityHandler);

    const instance = this.unityService.getInstance();
  if (instance) {
    // Detiene completamente el audio al salir de la ruta
    instance.SendMessage('GameManager', 'DetenerAudio', 'true');
    setTimeout(() => {
      instance.Quit().then(() => {
        this.unityService.setInstance(null);
      });
    }, 200);
  }
  }

  handleUnityMessage(event: MessageEvent): void {
  if (event.data?.type === 'GAME_OVER') {
    const { level_id, moves, time_ms } = event.data;

    this.score.guardarScore({ level_id, moves, time_ms }).subscribe({
      next: (res) => {
        this.score.getScoreUser().subscribe({
          next: (completats) => {
            this.nivellsCompletats = [...completats]; // reactividad
          },
          error: (err) => console.log(err)
        });
      },
      error: (err) => console.error('Error guardant score:', err)
    });
  }
}

  isNivellDesbloquejat(levelId: number): boolean {
    if (levelId === 6) {
    return this.nivellsCompletats.includes(levelId);
  }

    if(levelId === 1 ) return true;
   for (let i = 1; i < levelId; i++) {
    if (!this.nivellsCompletats.includes(i)) return false;
  }
  return true;
  }
}
