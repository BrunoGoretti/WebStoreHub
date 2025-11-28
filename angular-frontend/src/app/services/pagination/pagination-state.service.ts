import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { BehaviorSubject, filter } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PaginationStateService {
  private currentPageSubject = new BehaviorSubject<number>(1);
  currentPage$ = this.currentPageSubject.asObservable();

  constructor(private router: Router, private route: ActivatedRoute) {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        const pageFormUrl = this.route.snapshot.queryParamMap.get('page');
        const pageNum = pageFormUrl ? +pageFormUrl : 1;
        if (pageNum !== this.currentPageSubject.value) {
          this, this.currentPageSubject.next(pageNum);
        }
      });
  }

  setPage(page: number) {
    if (page !== this.currentPageSubject.value) {
      this.currentPageSubject.next(page);
      this.updateUrl(page);
    }
  }

  private updateUrl(page: number) {
    this.router.navigate([], {
      queryParams: { page },
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });
  }
}

