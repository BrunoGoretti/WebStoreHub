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
  public currentTypeSubject = new BehaviorSubject<string>('');
  currentType$ = this.currentTypeSubject.asObservable();

  constructor(private router: Router, private route: ActivatedRoute) {
    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        const page = +(this.route.snapshot.queryParamMap.get('page') ?? 1);
        const sort = this.route.snapshot.queryParamMap.get('sort') ?? '';
        const type = this.route.snapshot.queryParamMap.get('type') ?? '';

        if (page !== this.currentPageSubject.value) {
          this.currentPageSubject.next(page);
        }

        if (sort !== this.currentSortSubject.value) {
          this.currentSortSubject.next(sort);
        }
        if (type !== this.currentTypeSubject.value) {
          this.currentTypeSubject.next(type);
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

 setType(type: string) {
  if (type === this.currentTypeSubject.value) {
    return;
  }
  this.currentTypeSubject.next(type);
  this.updateUrl();
}

  private updateUrl() {
    this.router.navigate([], {
      queryParams: {
        page: this.currentPageSubject.value,
        sort: this.currentSortSubject.value,
        type: this.currentTypeSubject.value,
      },
      queryParamsHandling: 'merge',
      replaceUrl: true,
    });
  }
}
