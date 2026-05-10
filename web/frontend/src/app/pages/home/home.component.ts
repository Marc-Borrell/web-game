import { Component, inject, OnInit } from '@angular/core';
import { Auth } from '../../core/services/auth.service';
import { Router } from '@angular/router';
import { Navbar } from '../../shared/components/navbar/navbar.component';
import { Footer } from '../../shared/components/footer/footer.component';
import { UnityService } from '../../core/services/unity.service';

@Component({
  selector: 'app-home',
  imports: [Navbar, Footer],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class Home implements OnInit {

  constructor(private unityService: UnityService) {

  }

  private authService = inject(Auth);
  private router = inject(Router);

  public usuari: String = this.authService.getUser().name;


  ngOnInit() {

    if (this.unityService.getInstance()) return;
    const token = this.authService.getToken(); // el JWT que guardas en localStorage/sessionStorage

    const username = this.authService.getUser().name;

    //@ts-ignore
    createUnityInstance(document.querySelector("#unity-canvas"), {
      dataUrl: "/unity/Build/juegoxd.data",
      frameworkUrl: "/unity/Build/juegoxd.framework.js",
      codeUrl: "/unity/Build/juegoxd.wasm",
      streamingAssetsUrl: "StreamingAssets",
      companyName: "QQClan",
      productName: "NOM-Protocol",
      productVersion: "1.0"
    }).then((unityInstance: any) => {

      //this.unityService.setInstance(unityInstance);

      const payload = JSON.stringify({ token, username });

      (window as any).onUnityReady = () => {
        this.unityService.sendMessage('GameManager', 'SetAuthToken', payload);
      };
      setTimeout(() => {
        this.unityService.sendMessage('GameManager', 'SetAuthToken', payload);
      }, 3000);

      setTimeout(() => {
        this.unityService.sendMessage('GameManager', 'SetAuthToken', payload);

        // Si hay un nivel pendiente, lo cargamos
        if (this.unityService.pendingLevel) {
          this.unityService.sendMessage('GameManager', 'LoadLevel', this.unityService.pendingLevel);
          this.unityService.pendingLevel = null;
        }
      }, 3000);
    });

  }

}
