﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleBooksWPF.Helpers {
	public interface INavigationService {
		void NavigateTo(Uri uri);
	}
}