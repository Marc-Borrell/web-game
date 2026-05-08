import { Component, inject } from '@angular/core';
import { Auth } from '../../core/services/auth.service';
import { Router } from '@angular/router';
import { Navbar } from '../../shared/components/navbar/navbar.component';
import { Footer } from '../../shared/components/footer/footer.component';

@Component({
  selector: 'app-home',
  imports: [Navbar, Footer],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class Home {

  private authService = inject(Auth);
  private router = inject(Router);

  public usuari: String = this.authService.getUser().name; 

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  ngOnInit() {
        const token = this.authService.getToken(); // el JWT que guardas en localStorage/sessionStorage

        const username = 

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
    // Cuando Unity está listo, le mandamos el token
    unityInstance.SendMessage('GameManager', 'SetAuthToken', token);
  });
  }

}
