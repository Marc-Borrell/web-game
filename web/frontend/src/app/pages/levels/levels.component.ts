import { Component, OnInit } from '@angular/core';
import { Navbar } from '../../shared/components/navbar/navbar.component';
import { Footer } from '../../shared/components/footer/footer.component';
import { UnityService } from '../../core/services/unity.service';
import { Level } from '../../core/services/level.service';
import { Router } from '@angular/router';
import { Auth } from '../../core/services/auth.service';

@Component({
  selector: 'app-levels',
  imports: [Navbar, Footer],
  templateUrl: './levels.component.html',
  styleUrl: './levels.component.scss',
})
export class Levels implements OnInit {

  levels: { id: number, name: string }[] = [];

  constructor(
    private unityService: UnityService,
    private levelsService: Level,
    private authService: Auth
  ) {}

  ngOnInit() {
  this.levelsService.getLevels().subscribe(data => {
    this.levels = data;
  });

  // Siempre reiniciamos porque el canvas es nuevo cada vez
  this.unityService.setInstance(null);

  const token = this.authService.getToken();
  const username = this.authService.getUser().name;

  // @ts-ignore
  createUnityInstance(document.querySelector("#unity-canvas"), {
    dataUrl: "/unity/Build/juegoxd.data",
    frameworkUrl: "/unity/Build/juegoxd.framework.js",
    codeUrl: "/unity/Build/juegoxd.wasm",
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
}
