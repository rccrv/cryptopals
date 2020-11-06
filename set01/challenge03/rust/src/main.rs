use std::collections::HashMap;
use std::num::ParseIntError;

const ALPHABET: [(char, f64); 26] = [
    ('a', 0.08167), ('b', 0.01492), ('c', 0.02782),
    ('d', 0.04253), ('e', 0.12702), ('f', 0.02228),
    ('g', 0.02015), ('h', 0.06094), ('i', 0.06966),
    ('j', 0.00153), ('k', 0.00772), ('l', 0.04025),
    ('m', 0.02406), ('n', 0.06749), ('o', 0.07507),
    ('p', 0.01929), ('q', 0.00095), ('r', 0.05987),
    ('s', 0.06327), ('t', 0.09056), ('u', 0.02758),
    ('v', 0.00978), ('w', 0.02360), ('x', 0.00150),
    ('y', 0.01974), ('z', 0.00074)
    ];

fn qui_square(xored: String) -> f64 {
    let l = xored.len() as f64;
    let _a = 'a' as u8;
    let mut count: [u16; 26] = [0; 26];
    let mut sum: f64 = 0.0;

    for c in xored.chars() {
        match c {
            c if c.is_ascii_alphabetic() => {
                let nc = (c as u8 - _a) as usize;
                count[nc] += 1;
            },
            c if c.is_ascii_digit() => sum += 5.0,
            c if c.is_ascii_whitespace() => sum += 0.1,
            c if c.is_ascii_punctuation() =>  sum += 5.0,
            _ => {
                return f64::INFINITY;
            }
        }
    }

    for i in 0..count.len() {
        let o = count[i] as f64;
        let e = ALPHABET[i].1 * l;
        sum += (o - e) * (o - e) / e;
    }
    sum
}

fn analysis(m: &HashMap<char, String>) -> (char, f64) {
    let mut bestfit: (char, f64) = ('\0', f64::INFINITY);
    for (c, s) in m.iter() {
        let n = qui_square(s.clone().to_lowercase());
        if n < bestfit.1 {
            bestfit = (*c, n);
        }
    }

    bestfit
}

fn main() {
    let s = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
    let mut v: Vec<u8> = Vec::new();
    let mut map: HashMap<char, String> = HashMap::new();
    let _0 = '0' as u8;
    let _9 = '9' as u8;
    let _a = 'a' as u8;
    let _z = 'z' as u8;
    let _aup = 'A' as u8;
    let _zup = 'Z' as u8;

    let bytes = match (0..s.len()).step_by(2)
    .map(|i| u8::from_str_radix(&s[i..i+2], 16))
    .collect::<Result<Vec<u8>, ParseIntError>>() {
        Ok(o) => o,
        Err(e) => panic!("Couldn't parse {}: {}", s, e)
    };

    v.extend(_0..(_9 + 1));
    v.extend(_a..(_z + 1));
    v.extend(_aup..(_zup + 1));

    for i in v {
        map.insert(char::from(i), bytes.iter().map(|b| char::from(*b ^ i)).collect());
    }
    let c = analysis(&map);
    println!("Decoded by using {}\nResult: {}", c.0, map[&c.0]);
}
