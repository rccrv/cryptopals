use std::env;
use std::fs::File;
use std::io::BufReader;
use std::io::prelude::*;
use std::num::ParseIntError;
use std::path::Path;

const ALPHABET: [(char, f64); 26] = [
    ('a', 0.08167),
    ('b', 0.01492),
    ('c', 0.02782),
    ('d', 0.04253),
    ('e', 0.12702),
    ('f', 0.02228),
    ('g', 0.02015),
    ('h', 0.06094),
    ('i', 0.06966),
    ('j', 0.00153),
    ('k', 0.00772),
    ('l', 0.04025),
    ('m', 0.02406),
    ('n', 0.06749),
    ('o', 0.07507),
    ('p', 0.01929),
    ('q', 0.00095),
    ('r', 0.05987),
    ('s', 0.06327),
    ('t', 0.09056),
    ('u', 0.02758),
    ('v', 0.00978),
    ('w', 0.02360),
    ('x', 0.00150),
    ('y', 0.01974),
    ('z', 0.00074),
];

fn chisquare(xored: String) -> f64 {
    let mut count: [u16; 26] = [0; 26];
    let mut sum: f64 = 0.0;

    for c in xored.chars() {
        match c {
            c if c.is_ascii_alphabetic() => count[((c as u8) - ('a' as u8)) as usize] += 1,
            c if c.is_ascii_digit() => sum += 5.0,
            c if c.is_ascii_whitespace() => sum += 0.1,
            c if c.is_ascii_punctuation() => sum += 5.0,
            _ => return f64::INFINITY,
        }
    }

    for i in 0..26 {
        let o = count[i] as f64;
        let e = ALPHABET[i].1 * (xored.len() as f64);
        sum += (o - e) * (o - e) / e;
    }
    sum
}

fn analyzestring(s: &str) -> (char, f64, String) {
    let mut v: Vec<u8> = Vec::new();
    let mut bestfit = ('\0', f64::INFINITY, String::new());
    v.extend(('0' as u8)..(('9' as u8) + 1));
    v.extend(('a' as u8)..(('z' as u8) + 1));
    v.extend(('A' as u8)..(('Z' as u8) + 1));

    let bytes = match (0..s.len())
        .step_by(2)
        .map(|i| u8::from_str_radix(&s[i..i + 2], 16))
        .collect::<Result<Vec<u8>, ParseIntError>>()
    {
        Ok(o) => o,
        Err(e) => panic!("Couldn't parse {}: {}", s, e),
    };

    for i in v {
        let xored: String = bytes.iter().map(|b| char::from(*b ^ i)).collect();
        let n = chisquare(xored.clone().to_lowercase());
        if n < bestfit.1 {
            bestfit = (char::from(i), n, xored);
        }
    }

    bestfit
}

fn main() {
    let args: Vec<String> = env::args().collect();
    let mut bestfit = ('\0', f64::INFINITY, String::new());
    let mut oline: String = String::new();
    for fname in &args[1..] {
        let path = Path::new(fname);
        let file = match File::open(&path) {
            Err(e) => panic!("couldn't open {}: {}", path.display(), e),
            Ok(file) => file
        };
        let reader = BufReader::new(file);

        for l in reader.lines() {
            if let Ok(line) = l {
                let r = analyzestring(&line[..]);
                if r.1 < bestfit.1 {
                    bestfit = r;
                    oline = line;
                }
            }
        }
    }
    println!("Decoded by using {}\nOriginal: {}\nResult: {}", bestfit.0, oline, bestfit.2);
}