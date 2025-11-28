import { Injectable } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { BehaviorSubject, filter } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PaginationStateService {
  public currentPageSubject = new BehaviorSubject<number>(1);
  currentPage$ = this.currentPageSubject.asObservable();
  public currentSortSubject = new BehaviorSubject<string>('');
  currentSort$ = this.currentSortSubject.asObservable();

  constructor(private router: Router, private route: ActivatedRoute) {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        const page = +(this.route.snapshot.queryParamMap.get('page') ?? 1);
        const sort = this.route.snapshot.queryParamMap.get('sort') ?? '';

        if (page !== this.currentPageSubject.value) {
          this.currentPageSubject.next(page);
        }

        if (sort !== this.currentSortSubject.value) {
          this.currentSortSubject.next(sort);
        }
      });
  }

  setPage(page: number) {
    this.currentPageSubject.next(page);
    this.updateUrl();
  }

  setSort(sort: string) {
    if (sort === this.currentSortSubject.value) {
      return;
    }
    this.currentSortSubject.next(sort);
    this.updateUrl();
  }

  private updateUrl() {
    this.router.navigate([], {
      queryParams: {
        page: this.currentPageSubject.value,
        sort: this.currentSortSubject.value,
      },
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });
  }
}
