/* ************************************************************************** */
/*                                                                            */
/*                                                        :::      ::::::::   */
/*   ft_split.c                                         :+:      :+:    :+:   */
/*                                                    +:+ +:+         +:+     */
/*   By: caide-so <caide-so@student.42sp.org.br>    +#+  +:+       +#+        */
/*                                                +#+#+#+#+#+   +#+           */
/*   Created: 2024/10/16 21:24:25 by caide-so          #+#    #+#             */
/*   Updated: 2025/01/01 20:48:51 by caide-so         ###   ########.fr       */
/*                                                                            */
/* ************************************************************************** */

#include "../../include/libft.h"

static int	split_loop(char const *s, char c, char **res);
static int	process_word(const char *substr, size_t len, char **res, size_t *j);

char	**ft_split(char const *s, char c)
{
	char	**res;
	size_t	word_count;

	if (!s)
		return (NULL);
	word_count = ft_word_count(s, c);
	res = (char **)malloc((word_count + 1) * sizeof(char *));
	if (!res)
		return (NULL);
	if (!split_loop(s, c, res))
	{
		free(res);
		return (NULL);
	}
	return (res);
}

static int	split_loop(char const *s, char c, char **res)
{
	size_t	i;
	size_t	j;
	size_t	start;
	size_t	len;

	i = 0;
	j = 0;
	start = 0;
	while (s[i])
	{
		if ((i == 0 || s[i - 1] == c) && s[i] != c)
			start = i;
		if (s[i] != c && (s[i + 1] == c || !s[i + 1]))
		{
			len = i - start + 1;
			if (!process_word(s + start, len, res, &j))
				return (0);
		}
		i++;
	}
	res[j] = NULL;
	return (1);
}

static int	process_word(const char *substr, size_t len, char **res, size_t *j)
{
	res[*j] = (char *)malloc(len + 1);
	if (!res[*j])
		return (0);
	ft_strlcpy(res[*j], substr, len + 1);
	(*j)++;
	return (1);
}
