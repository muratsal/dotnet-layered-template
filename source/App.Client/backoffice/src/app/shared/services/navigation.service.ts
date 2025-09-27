import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { BehaviorSubject } from 'rxjs';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

interface IMenuItem {
  type: 'link' | 'dropDown' | 'icon' | 'separator' | 'extLink';
  name?: string; // Used as display text for item and title for separator type
  state?: string; // Router state
  icon?: string; // Material icon name
  svgIcon?: string; // UI Lib icon name
  disabled?: boolean; // If true, item will not be appeared in sidenav.
  sub?: IChildItem[]; // Dropdown items
  badges?: IBadge[];
}
interface IChildItem {
  type?: string;
  name: string; // Display text
  state?: string; // Router state
  icon?: string;  // Material icon name
  svgIcon?: string; // UI Lib icon name
  sub?: IChildItem[];
}

interface IBadge {
  color: string; // primary/accent/warn/hex color codes(#fff000)
  value: string; // Display text
}

@Injectable({
  providedIn: 'root'
})
export class NavigationService {


  plainMenu: IMenuItem[] = [
    {
      name: 'DASHBOARD',
      type: 'dropDown',
      icon: 'dashboard',
      sub: [
        { name: 'Analytics', state: 'dashboard/analytics' },
        { name: 'Learning Management', state: 'dashboard/learning-management' },
        { name: 'Analytics Alt', state: 'dashboard/analytics-alt' },
        { name: 'Cryptocurrency', state: 'dashboard/crypto' },
      ]
    },
    {
      name: 'Material Kits',
      type: 'dropDown',
      icon: 'favorite',
      badges: [{ color: 'primary', value: '60+' }],
      sub: [
        {
          name: 'Form controls',
          type: 'dropDown',
          sub: [
            { name: 'Autocomplete', state: 'material/autocomplete' }
          ]
        }
      ]
    },
    {
      name: 'DOC',
      type: 'extLink',
      icon: 'library_books',
      state: 'http://demos.ui-lib.com/egret-doc/'
    }
  ];

  // Icon menu TITLE at the very top of navigation.
  // This title will appear if any icon type item is present in menu.
  iconTypeMenuTitle = 'Frequently Accessed';
  // sets iconMenu as default;
  menuItems = new BehaviorSubject<IMenuItem[]>(this.plainMenu);
  // navigation component has subscribed to this Observable
  // menuItems$ = this.menuItems.asObservable();

  menuItems$ = this.httpClient.get(`${environment.apiURL}/Login/GetUserMenu`,  {
        headers: new HttpHeaders({
            'Content-Type': 'application/json; charset=utf-8'
        })
    })
    .pipe(map((response: any) => {

        let menuItems: IMenuItem[] = [];

        response.userMenuInfo.forEach(x => {

            menuItems.push({
                name: x.translateName,
                type: 'dropDown',
                icon: x.menuIcon,
                state: x.state,
                sub: x.subMenuItems.map(y =>
                { 
                  if(!y.subMenuItems || y.subMenuItems.length==0)
                    return { name: y.translateName, state: y.status }
                  else
                  {
                    return { name: y.translateName,
                        type:'dropDown',
                        sub: y.subMenuItems.map(
                        z=> {
                          return { name: z.translateName, state: z.status }
                        }
                        )
                    }
                  }
                }
                )
            });

        });

        return menuItems;
    })
    );

  constructor(private httpClient: HttpClient) { }

  // Customizer component uses this method to change menu.
  // You can remove this method and customizer component.
  // Or you can customize this method to supply different menu for
  // different user type.
  publishNavigationChange(menuType: string) {
    // switch (menuType) {
    //   case 'separator-menu':
    //     this.menuItems.next(this.separatorMenu);
    //     break;
    //   case 'icon-menu':
    //     this.menuItems.next(this.iconMenu);
    //     break;
    //   default:
    //     this.menuItems.next(this.plainMenu);
    // }
  }
}
