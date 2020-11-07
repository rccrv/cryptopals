use std::num::ParseIntError;

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

fn chi_square(xored: String) -> f64 {
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

fn main() {
    let s = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
    let mut v: Vec<u8> = Vec::new();
    let mut r = String::new();
    let mut bestfit: (char, f64) = ('\0', f64::INFINITY);
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
        let n = chi_square(xored.clone().to_lowercase());
        if n < bestfit.1 {
            bestfit = (char::from(i), n);
            r = xored;
        }
    }
    println!("Decoded by using {}\nResult: {}", bestfit.0, r);
}
