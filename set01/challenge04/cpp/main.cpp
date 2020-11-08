#include <algorithm>
#include <cctype>
#include <cstdint>
#include <fstream>
#include <iostream>
#include <limits>
#include <string>
#include <tuple>
#include <vector>

using namespace std;

const tuple<char, double> ALPHABET[26] = {
    make_tuple('a', 0.08167), make_tuple('b', 0.01492),
    make_tuple('c', 0.02782), make_tuple('d', 0.04253),
    make_tuple('e', 0.12702), make_tuple('f', 0.02228),
    make_tuple('g', 0.02015), make_tuple('h', 0.06094),
    make_tuple('i', 0.06966), make_tuple('j', 0.00153),
    make_tuple('k', 0.00772), make_tuple('l', 0.04025),
    make_tuple('m', 0.02406), make_tuple('n', 0.06749),
    make_tuple('o', 0.07507), make_tuple('p', 0.01929),
    make_tuple('q', 0.00095), make_tuple('r', 0.05987),
    make_tuple('s', 0.06327), make_tuple('t', 0.09056),
    make_tuple('u', 0.02758), make_tuple('v', 0.00978),
    make_tuple('w', 0.02360), make_tuple('x', 0.00150),
    make_tuple('y', 0.01974), make_tuple('z', 0.00074),
};

double chisquare(string &xored) {
  double sum = 0.0;
  uint16_t count[26] = {0};

  for (char c : xored) {
    if (islower(c)) {
      count[c - 'a'] += 1;
    } else if (isdigit(c)) {
      sum += 5.0;
    } else if (isspace(c)) {
      sum += 0.1;
    } else if (ispunct(c)) {
      sum += 5.0;
    } else {
      return numeric_limits<double>::infinity();
    }
  }

  for (int i = 0; i < 26; i++) {
    auto o = count[i];
    auto e = get<1>(ALPHABET[i]) * xored.size();
    sum += (o - e) * (o - e) / e;
  }

  return sum;
}

tuple<char, double, string> analyzestring(const string &s) {
  tuple<char, double, string> bestfit =
      make_tuple('\0', numeric_limits<double>::infinity(), "");
  vector<uint8_t> bytes = {};
  vector<uint8_t> l = {};
  for (int i = 65; i < 65 + 26; i++) {
    l.push_back(i);
    l.push_back(i + 32);
  }
  for (int i = 48; i < 48 + 10; i++) {
    l.push_back(i);
  }

  for (int i = 0; i < s.length(); i += 2) {
    bytes.push_back(stoi("0x" + s.substr(i, 2), NULL, 16));
  }

  for (char c : l) {
    auto v = bytes;
    for_each(v.begin(), v.end(), [c](uint8_t &b) { b ^= c; });
    string xored = string(v.begin(), v.end());
    auto lower = xored;
    transform(lower.begin(), lower.end(), lower.begin(),
              [](unsigned char c) -> unsigned char { return tolower(c); });
    auto n = chisquare(lower);
    if (n < get<1>(bestfit)) {
      bestfit = make_tuple(c, n, xored);
    }
  }

  return bestfit;
}

int main(int argc, char *argv[]) {
  tuple<char, double, string> bestfit = make_tuple('\0', numeric_limits<double>::infinity(), "");

  if (argc >= 1) {
    string fname = argv[1];
    ifstream filein(fname);

    for (string line; getline(filein, line); ) {
      auto r = analyzestring(line);
      if (get<1>(r) < get<1>(bestfit)) {
        bestfit = r;
      }
    }
  }

  cout << "Decoded by using " << get<0>(bestfit) << "\nResult: " << get<2>(bestfit) << '\n';

  return 0;
}
